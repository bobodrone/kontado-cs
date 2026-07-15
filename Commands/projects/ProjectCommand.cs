using System.CommandLine;

namespace kontado_csharp.Commands.projects;

// The "project" command is a grouping shell: it has no action of its own.
// Its only job is to hold child commands ("list", "show").
// Running `kontado project` with no child makes the framework print help automatically.
public static class ProjectCommand
{
    public static Command Create()
    {
        var cmd = new Command("project", "Manage projects");
        cmd.Subcommands.Add(ListCommand.Create());
        cmd.Subcommands.Add(AddCommand.Create());
//        cmd.Subcommands.Add(ShowCommand.Create());
        return cmd;
    }
}