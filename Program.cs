using Spectre.Console;
using System.CommandLine;
using kontado_csharp.Commands; // for the greet command only!
using kontado_csharp.Commands.clients;
using kontado_csharp.Commands.projects;


var rootCommand = new RootCommand("Demo CLI using System.CommandLine + Spectre.Console");


// Add the "client" command group (holds list + show) and the greet command.
rootCommand.Subcommands.Add(ClientCommand.Create());
rootCommand.Subcommands.Add(ProjectCommand.Create());
rootCommand.Subcommands.Add(GreetCommand.Create());

var invocationConfig = new InvocationConfiguration
{
    Output = Console.Out,
    Error = Console.Error,
};

var parseResult = rootCommand.Parse(args);

return await parseResult.InvokeAsync(invocationConfig);