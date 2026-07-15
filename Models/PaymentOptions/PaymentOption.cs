using YamlDotNet.Serialization;

namespace kontado_csharp.Models.PaymentOptions;

// Abstract base for all payment options. Mirrors the Python `PaymentOption` base class.
// `Type` is the discriminator ("swish", "bankgiro", "bank_transfer") used by the
// PaymentOptionConverter to pick the right concrete subclass when deserializing.
public abstract class PaymentOption
{
    public string Label { get; set; } = "";
    public string Description { get; set; } = "";

    // Discriminator. Set by each subclass's WithDefaults() (e.g. "swish").
    [YamlMember(Alias = "type")]
    public string Type { get; set; } = "";

    // Each subclass overrides this to build a sensible default instance of itself.
    public abstract PaymentOption WithDefaults();
}