using Spectre.Console;
using System.CommandLine;
using kontado_csharp.Data;
using kontado_csharp.Models;

namespace kontado_csharp.Commands.projects;

public static class ListCommand
{
    public static Command Create()
    {

        var clientNameArgument = new Argument<string>("name")
        {
            Description = "Client name to show (omit to pick interactively)",
            DefaultValueFactory = _ => "",
        };

        var cmd = new Command("list", "List all projects for a client in a table")
        {
            clientNameArgument
        };

        cmd.SetAction((parseResult, _) =>
        {

            var clientName = parseResult.GetValue(clientNameArgument) ?? "";
            var clientRepo = new ClientRepository();

            Client? client;

            if (string.IsNullOrEmpty(clientName))
            {
                // No name given: build an interactive picker from all clients.
                var clients = clientRepo.GetAll();
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

                client = clientRepo.GetClient(pick);
            }
            else
            {
                client = clientRepo.GetClient(clientName);
            }

            if (client is null)
            {
                AnsiConsole.MarkupLine($"[red]No client named[/] [yellow]{Markup.Escape(clientName)}[/] [red]found.[/]");
                return Task.FromResult(1);
            }

            var projectRepo = new ProjectRepository();
            var projects = projectRepo.GetAll(client.Name);

            var table = new Table();
            table.AddColumn("[bold]Id[/]");
            table.AddColumn("[bold]Name[/]");
            table.AddColumn("[bold]Label[/]");

            foreach (var p in projects)
                table.AddRow(p.Id.ToString(), Markup.Escape(p.Name), Markup.Escape(p.Label));

            AnsiConsole.Write(table);
            return Task.FromResult(0);
        });
        return cmd;
    }
}