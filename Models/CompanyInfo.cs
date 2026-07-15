using System.Text.RegularExpressions;

namespace kontado_csharp.Models;

// "partial" because [GeneratedRegex] emits a second half of this class
// (a hidden *.g.cs file) at compile time. The compiler merges both halves.
public partial class CompanyInfo
{
    public string Name { get; set; } = "";
    public string OrgNr { get; set; } = "";
    public string VatNr { get; set; } = "";

    public Address? PostalAddress { get; set; } = null;
    public Address? VisitorAddress { get; set; } = null;


    public static CompanyInfo GetDefaults()
    {
        return new CompanyInfo {
            Name  = "company_name",
            OrgNr = "123456-7890",
            VatNr = "SE123456789001",
            PostalAddress = Address.GetDefaults(),
            VisitorAddress = Address.GetDefaults(),
        };
    }

}