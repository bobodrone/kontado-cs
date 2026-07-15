using YamlDotNet.Serialization;

namespace kontado_csharp.Models.PaymentOptions;

// Swish payment. Adds PhoneNumber. Discriminator: "swish".
public sealed class SwishPaymentOption : PaymentOption
{
    [YamlMember(Alias = "phone_number")]
    public string PhoneNumber { get; set; } = "";

    public override SwishPaymentOption WithDefaults() => new()
    {
        Label       = "Swish",
        Description = "Pay via Swish",
        Type        = "swish",
        PhoneNumber = "+46701234567",
    };
}