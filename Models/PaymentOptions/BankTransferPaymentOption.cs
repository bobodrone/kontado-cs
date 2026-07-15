using YamlDotNet.Serialization;

namespace kontado_csharp.Models.PaymentOptions;

// Bank transfer payment. Adds AccountNumber, ClearingNumber, Bank. Discriminator: "bank_transfer".
public sealed class BankTransferPaymentOption : PaymentOption
{
    [YamlMember(Alias = "account_number")]
    public string AccountNumber { get; set; } = "";

    [YamlMember(Alias = "clearing_number")]
    public string ClearingNumber { get; set; } = "";

    public string Bank { get; set; } = "";

    public override BankTransferPaymentOption WithDefaults() => new()
    {
        Label           = "Bank transfer",
        Description     = "Pay via bank transfer",
        Type            = "bank_transfer",
        AccountNumber   = "1234567",
        ClearingNumber  = "1234",
        Bank            = "Swedbank",
    };
}