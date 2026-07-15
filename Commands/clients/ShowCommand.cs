using Spectre.Console;
using System.CommandLine;
using kontado_csharp.Data;
using kontado_csharp.Models;

namespace kontado_csharp.Commands.clients;

// Renamed from ShowClientCommand -> ShowCommand.
// The "name" argument is optional: empty string means "pick interactively".
//   kontado client show volvo   -> look up "volvo" directly
//   kontado client show         -> list all names, let the user pick one
public static class ShowCommand
{
    public static Command Create()
    {
        var nameArgument = new Argument<string>("name")
        {
            Description = "Client name to show (omit to pick interactively)",
            DefaultValueFactory = _ => "",
        };

        var cmd = new Command("show", "Show a single client")
        {
            nameArgument
        };

        cmd.SetAction((parseResult, _) =>
        {
            var clientName = parseResult.GetValue(nameArgument) ?? "";
            var repo = new ClientRepository();

            Client? client;

            if (string.IsNullOrEmpty(clientName))
            {
                // No name given: build an interactive picker from all clients.
                var clients = repo.GetAll();
                if (clients.Count == 0)
                {
                    AnsiConsole.MarkupLine("[red]No clients found.[/]");
                    return Task.FromResult(1);
                }

                // SelectionPrompt blocks on stdin and lets the user arrow-key a choice.
                // Returns the chosen string (here, a client Name).
                var pick = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Pick a client:")
                        .AddChoices(clients.Select(c => c.Name)));

                client = repo.GetClient(pick);
            }
            else
            {
                client = repo.GetClient(clientName);
            }

            if (client is null)
            {
                AnsiConsole.MarkupLine($"[red]No client named[/] [yellow]{Markup.Escape(clientName)}[/] [red]found.[/]");
                return Task.FromResult(1);
            }

            var table = new Table();
            table.AddColumn("[bold]Property[/]");
            table.AddColumn("[bold]Value[/]");
            table.AddRow("Id",    client.Id.ToString());
            table.AddRow("Name",  Markup.Escape(client.Name));
            table.AddRow("Label", Markup.Escape(client.Label));
            AnsiConsole.Write(table);
            return Task.FromResult(0);
        });
        return cmd;
    }
}