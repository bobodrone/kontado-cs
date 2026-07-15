using System.Text.RegularExpressions;

namespace kontado_csharp.Models;

// "partial" because [GeneratedRegex] emits a second half of this class
// (a hidden *.g.cs file) at compile time. The compiler merges both halves.
public partial class Client : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Label { get; set; } = "";

    // A valid client "machine name" (later used as a folder name): one or more of
    // lowercase letters, digits, or underscore. Anchored with ^...$ so the WHOLE
    // string must match, not just a substring.
    [GeneratedRegex(@"^[a-z0-9_]+$")]
    private static partial Regex ValidNameRegex();

    // Public helper so commands and the repository can validate without duplicating the rule.
    public static bool IsValidName(string name) =>
        !string.IsNullOrEmpty(name) && ValidNameRegex().IsMatch(name);
}