namespace kontado_csharp.Models.PaymentOptions;

// Maps the `type:` discriminator string to concrete PaymentOption subclasses and back.
// Used by PaymentOptionConverter when serializing/deserializing YAML.
public static class PaymentOptionFactory
{
    // C# Type -> discriminator string (used when writing YAML).
    public static string Discriminator(Type t) => t switch
    {
        _ when t == typeof(SwishPaymentOption)        => "swish",
        _ when t == typeof(BankGiroPaymentOption)     => "bankgiro",
        _ when t == typeof(BankTransferPaymentOption) => "bank_transfer",
        _ => throw new InvalidOperationException(
            $"Unknown payment option type {t}")
    };

    // Discriminator string -> empty concrete instance (used when reading YAML;
    // YamlDotNet then fills the instance's properties).
    public static PaymentOption Create(string type) => type switch
    {
        "swish"         => new SwishPaymentOption(),
        "bankgiro"      => new BankGiroPaymentOption(),
        "bank_transfer" => new BankTransferPaymentOption(),
        _ => throw new InvalidOperationException(
            $"Unknown payment option type '{type}'")
    };
}