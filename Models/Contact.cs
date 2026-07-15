using System.Text.RegularExpressions;

namespace kontado_csharp.Models;

// "partial" because [GeneratedRegex] emits a second half of this class
// (a hidden *.g.cs file) at compile time. The compiler merges both halves.
public partial class Contact
{
    public string Name { get; set; } = "";
    public string Title { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public bool InvoiceContact { get; set; } = false;



    public static List<Contact> GetDefaults()
    {
        return [new Contact {
            Name  = "Ada Lovelace",
            Title = "Developer",
            Email = "ada@love.ly",
            Phone = "+467123456789",
            InvoiceContact = true,
        }];
    }

}