using Spectre.Console;
using System.CommandLine;

namespace kontado_csharp.Commands;

public static class GreetCommand
{



    public static Command Create()
    {
        var cmd = new Command("greet", "Greet people in the world");

        var nameOption = new Option<string>("--name")
        {
            Description = "Name to greet",
            DefaultValueFactory = _ => "world",
        };

        var countOption = new Option<int>("--count")
        {
            Description = "How many times to greet",
            DefaultValueFactory = _ => 1,
        };

        cmd.Add(nameOption);
        cmd.Add(countOption);


        cmd.SetAction((parseResult, _) =>
        {
            var name = parseResult.GetValue(nameOption) ?? "world";
            var count = parseResult.GetValue(countOption);

            AnsiConsole.MarkupLine("[blue]Starting greeting command...[/]");

            for (var i = 0; i < count; i++)
            {
                AnsiConsole.MarkupLine($"Hello, [green]{Markup.Escape(name)}[/]!");
            }

            return Task.FromResult(0);
        });
        return cmd;
    }
}