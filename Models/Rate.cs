using System.Text.RegularExpressions;

namespace kontado_csharp.Models;

// Rate is a value object owned by a Task (no IEntity — no Id, no own file).
// Two GetDefaults overloads:
//   - Rate.GetDefaults(amount, vat)    -> a billed rate
//   - Rate.GetDefaults()                -> a zero/"free" rate
public class Rate
{
    public int Amount { get; set; } = 0;
    public int Vat { get; set; } = 0;
    public string Currency { get; set; } = "SEK";
    public string Type { get; set; } = "";

    public static Rate GetDefaults(int amount = 0, int vat = 0  ) =>
        new() { Amount = amount, Vat = vat, Currency = "SEK", Type = "hourly" };
}