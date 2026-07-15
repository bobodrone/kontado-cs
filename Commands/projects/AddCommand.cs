using Spectre.Console;
using System.CommandLine;
using kontado_csharp.Data;
using kontado_csharp.Models;

namespace kontado_csharp.Commands.projects;

public static partial class AddCommand
{
    public static Command Create()
    {
        var clientNameArgument = new Argument<string>("name")
        {
            Description = "Client name to show (omit to pick interactively)",
            DefaultValueFactory = _ => "",
        };

        var cmd = new Command("add", "List all projects for a client in a table")
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

            var name = AnsiConsole.Prompt(new TextPrompt<string>($"[green]Type the project machine name (a-z0-9_):[/]"));
            if (!Project.IsValidName(name))
            {
                AnsiConsole.MarkupLine($"[red]Invalid project name. Use only a-z, 0-9 and _.[/]");
                return Task.FromResult(1);
            }
            var label = AnsiConsole.Prompt(new TextPrompt<string>($"[green]Type the project label:[/]"));
            if (string.IsNullOrEmpty(label))
            {
                AnsiConsole.MarkupLine($"[red]No project label added.[/]");
                return Task.FromResult(1);
            }

            var project = new Project
            {
                Id    = projectRepo.NextId(client.Name),
                Name  = name,
                Label = label,
                Client = client.Name
            };

            try
            {
                projectRepo.Add(client.Name, project, slug: name);
                AnsiConsole.MarkupLine($"[green]Added[/] [yellow]{Markup.Escape(label)}[/] as [yellow]{Markup.Escape(name)}[/] (id {project.Id}).");
            }
            catch (InvalidOperationException ex)
            {
                AnsiConsole.MarkupLine($"[red]{Markup.Escape(ex.Message)}[/]");
                return Task.FromResult(1);
            }

            return Task.FromResult(0);
        });
        return cmd;
    }
}