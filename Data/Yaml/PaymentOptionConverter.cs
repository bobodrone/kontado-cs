using System.Reflection;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.TypeInspectors;
using kontado_csharp.Models.PaymentOptions;

namespace kontado_csharp.Data.Yaml;

// Custom YamlDotNet converter for PaymentOption (and its subclasses).
//
// What it achieves: emits/reads a FLAT YAML mapping that includes a `type:` field
// which acts as a discriminator, so a List<PaymentOption> can round-trip even though
// the concrete subclass is different per entry. This mirrors what Pydantic discriminated
// unions do in the Python original.
//
// To register on any YamlStore<T>/NestedYamlStore<T> subclass that stores PaymentOptions:
//   protected override IDeserializer MakeDeserializer() =>
//       new DeserializerBuilder()
//           .WithNamingConvention(CamelCaseNamingConvention.Instance)
//           .WithTypeConverter(new PaymentOptionConverter())
//           .Build();
//   (and the same for MakeSerializer)
public sealed class PaymentOptionConverter : IYamlTypeConverter
{
    public bool Accepts(Type type) => typeof(PaymentOption).IsAssignableFrom(type);

    // ---- Read: YAML -> C# ----
    // Walk the YAML mapping, collect each key/value into a dict, then dispatch on `type`.
    public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        if (!parser.TryConsume<MappingStart>(out _))
            throw new YamlException("Expected YAML mapping for a PaymentOption");

        var values = new Dictionary<string, object?>();
        while (!parser.TryConsume<MappingEnd>(out _))
        {
            var key = parser.Consume<Scalar>().Value;
            // We use a scalar-string parse here for simplicity; nested values (if any)
            // could be handled by reading the next event. For our flat option shapes
            // everything is scalar.
            var value = parser.Consume<Scalar>().Value;
            values[key] = value;
        }

        if (!values.TryGetValue("type", out var typeObj) || typeObj is not string typeStr)
            throw new YamlException("PaymentOption YAML missing required `type` field");

        var option = PaymentOptionFactory.Create(typeStr);

        // Fill properties of the concrete subclass from the dict.
        // Uses PascalCase property names AND the snake_case aliases declared via [YamlMember].
        foreach (var (key, value) in values)
        {
            if (key == "type") { option.Type = (string)value!; continue; }
            var prop = FindProperty(option.GetType(), key);
            if (prop is null) continue;
            prop.SetValue(option, value);
        }

        return option;
    }

    // ---- Write: C# -> YAML ----
    // Emit a flat mapping: first `type: <discriminator>`, then every public property
    // of the concrete type that isn't `Type` itself.
    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer rootSerializer)
    {
        if (value is null) return;
        var option = (PaymentOption)value;
        var concrete = option.GetType();

        emitter.Emit(new MappingStart());

        // type first, so a human reading the file knows the shape immediately.
        emitter.Emit(new Scalar("type"));
        emitter.Emit(new Scalar(PaymentOptionFactory.Discriminator(concrete)));

        // Then every other public gettable property, using its [YamlMember] alias if present
        // (so PhoneNumber -> phone_number, AccountNumber -> account_number, etc.).
        foreach (var prop in concrete.GetProperties())
        {
            if (prop.Name == nameof(PaymentOption.Type)) continue;
            var alias = prop.GetCustomAttribute<YamlMemberAttribute>()?.Alias ?? prop.Name;
            emitter.Emit(new Scalar(alias));
            emitter.Emit(new Scalar(prop.GetValue(option)?.ToString() ?? ""));
        }

        emitter.Emit(new MappingEnd());
    }

    // Resolve a property by its C# name OR its [YamlMember] snake_case alias.
    private static PropertyInfo? FindProperty(Type t, string key) =>
        t.GetProperties().FirstOrDefault(p =>
            p.Name.Equals(key, StringComparison.OrdinalIgnoreCase) ||
            p.GetCustomAttribute<YamlMemberAttribute>()?.Alias == key);
}