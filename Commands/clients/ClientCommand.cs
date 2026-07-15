using System.CommandLine;

namespace kontado_csharp.Commands.clients;

// The "client" command is a grouping shell: it has no action of its own.
// Its only job is to hold child commands ("list", "show").
// Running `kontado client` with no child makes the framework print help automatically.
public static class ClientCommand
{
    public static Command Create()
    {
        var cmd = new Command("client", "Manage clients");
        cmd.Subcommands.Add(ListCommand.Create());
        cmd.Subcommands.Add(ShowCommand.Create());
        cmd.Subcommands.Add(AddCommand.Create());
        return cmd;
    }
}