using KitCli;
using KitCli.Commands.Abstractions.Io.Outcomes;
using KitCli.Workflow.Abstractions;

namespace YnabSharp.Seeder;

public class YnabSharpSeederCliApp : CliApp
{
    public YnabSharpSeederCliApp(ICliWorkflow workflow, ICliCommandOutcomeIo io) : base(workflow, io)
    {
    }

    protected override void OnSessionStart()
    {
        Console.WriteLine("Welcome to the YNAB Seeder");
    }
}