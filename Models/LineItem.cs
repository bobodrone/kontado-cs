using System.Security.AccessControl;
using System.Text.RegularExpressions;

namespace kontado_csharp.Models;

public partial class LineItem
{

    public enum LineItemType
    {
        Service,
        Product,
    }

    public string Id { get; set; } = "";
    public string Client { get; set; } = "";
    public string Project { get; set; } = "";
    public LineItemType Type { get; set; } = LineItemType.Service;
    public string Description { get; set; } = "";
    public float Nth { get; set; } = 0;
    public int UnitPrice { get; set; } = 0;
    public string Currency { get; set; } = "";
    public string Task { get; set; } = "";
    public int Vat { get; set; } = 0;
    public int Netto { get; set; } = 0;
    public DateTime? Created{ get; set; } = null;
    public DateTime? Changed{ get; set; } = null;
    public bool Invoiced { get; set; } = false;

    public static LineItem GetDefaults()
    {
        return new LineItem {
            Id  = "proj-20221023-0001",
            Client = "Client 242",
            Project = "website",
            Task = "development",
            Type = LineItemType.Service,
            Description = "Website work",
            Nth = 1,
            UnitPrice = 100000,
            Currency = "SEK",
            Vat = 25,
            Netto = 100000,
            Created = DateTime.Now,
            Changed = DateTime.Now,
            Invoiced = false,
        };
    }    
}