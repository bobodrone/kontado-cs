using System.Security.AccessControl;
using System.Text.RegularExpressions;

namespace kontado_csharp.Models;

public partial class WorkTask
{
    public string Type { get; set; } = "";
    public string Label { get; set; } = "";
    public bool Default { get; set; } = false;
    public Rate? Rate { get; set; } = null;

    public static List<WorkTask> GetDefaults(int amount, int vat)
    {
        return [
            new WorkTask {
                Type  = "development",
                Label = "Development",
                Default = true,
                Rate = Rate.GetDefaults(amount, vat),
            },
            new WorkTask {
                Type  = "development_free",
                Label = "Development (free)",
                Default = false,
                Rate = Rate.GetDefaults(),
            },
            new WorkTask {
                Type  = "administration",
                Label = "Administration",
                Default = false,
                Rate = Rate.GetDefaults(amount, vat),
            },
            new WorkTask {
                Type  = "administration_free",
                Label = "Administration (free)",
                Default = false,
                Rate = Rate.GetDefaults(),
            }
        ];
    }
}