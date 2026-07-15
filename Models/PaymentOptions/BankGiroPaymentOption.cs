using YamlDotNet.Serialization;

namespace kontado_csharp.Models.PaymentOptions;

// Bankgiro payment. Adds AccountNumber. Discriminator: "bankgiro".
public sealed class BankGiroPaymentOption : PaymentOption
{
    [YamlMember(Alias = "account_number")]
    public string AccountNumber { get; set; } = "";

    public override BankGiroPaymentOption WithDefaults() => new()
    {
        Label         = "Bankgiro",
        Description   = "Pay via Bankgiro",
        Type          = "bankgiro",
        AccountNumber = "123-4567",
    };
}