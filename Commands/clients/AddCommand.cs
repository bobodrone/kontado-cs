using Spectre.Console;
using System.CommandLine;
using kontado_csharp.Data;
using kontado_csharp.Models;

namespace kontado_csharp.Commands.clients;

public static partial class AddCommand
{
    public static Command Create()
    {
 
        var cmd = new Command("add", "Add a new client");

        cmd.SetAction((_, _) =>
        {
            var repo = new ClientRepository();

            var name = AnsiConsole.Prompt(new TextPrompt<string>($"[green]Type the client machine name (a-z0-9_):[/]"));
            if (!Client.IsValidName(name))
            {
                AnsiConsole.MarkupLine($"[red]Invalid client name. Use only a-z, 0-9 and _.[/]");
                return Task.FromResult(1);
            }
            var label = AnsiConsole.Prompt(new TextPrompt<string>($"[green]Type the client real name:[/]"));
            if (string.IsNullOrEmpty(label))
            {
                AnsiConsole.MarkupLine($"[red]No client label added.[/]");
                return Task.FromResult(1);
            }

            var client = Client.GetDefaults();
            client.Id    = repo.NextId();
            client.Name  = name;
            client.Label = label;

            try
            {
                repo.Add(client, slug: name);
                AnsiConsole.MarkupLine($"[green]Added[/] [yellow]{Markup.Escape(label)}[/] as [yellow]{Markup.Escape(name)}[/] (id {client.Id}).");
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