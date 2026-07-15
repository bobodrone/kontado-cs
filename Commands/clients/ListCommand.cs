using Spectre.Console;
using System.CommandLine;
using kontado_csharp.Data;

namespace kontado_csharp.Commands.clients;

// Renamed from ListClientsCommand -> ListCommand.
// Command string changed from "list-clients" -> "list",
// because under the "client" group "clients" would be redundant:
//   kontado client list    (reads well)
//   kontado client list-clients  (awkward)
public static class ListCommand
{
    public static Command Create()
    {
        var cmd = new Command("list", "List all clients in a table");
        cmd.SetAction((_, _) =>
        {
            var repo = new ClientRepository();
            var clients = repo.GetAll();

            var table = new Table();
            table.AddColumn("[bold]Id[/]");
            table.AddColumn("[bold]Name[/]");
            table.AddColumn("[bold]Label[/]");

            foreach (var c in clients)
                table.AddRow(c.Id.ToString(), Markup.Escape(c.Name), Markup.Escape(c.Label));

            AnsiConsole.Write(table);
            return Task.FromResult(0);
        });
        return cmd;
    }
}