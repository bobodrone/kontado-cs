using System.Text.RegularExpressions;

namespace kontado_csharp.Models;

// "partial" because [GeneratedRegex] emits a second half of this class
// (a hidden *.g.cs file) at compile time. The compiler merges both halves.
public partial class Address
{
    public string Co { get; set; } = "";
    public string Thoroughfare { get; set; } = "";
    public string? Premise { get; set; } = null;

    public string PostalCode { get; set; } = "";
    public string Locality { get; set; } = "";
    public string Region { get; set; } = "";
    public string Country { get; set; } = "";

    public static Address GetDefaults()
    {
        return new Address {
            Co  = "company_name",
            Thoroughfare = "My street 242",
            Premise = null,
            PostalCode = "123 45",
            Locality = "My city",
            Region = "My region",
            Country = "My country",
        };
    }

}