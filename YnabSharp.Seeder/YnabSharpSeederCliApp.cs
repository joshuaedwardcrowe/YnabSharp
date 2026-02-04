using KitCli;
using KitCli.Abstractions.Io;
using KitCli.Commands.Abstractions.Outcomes;
using KitCli.Workflow.Abstractions;

namespace YnabSharp.Seeder;

public class YnabSharpSeederCliApp(ICliWorkflow workflow, ICliIo io) : CliApp(workflow, io)
{
    protected override void OnSessionStart()
    {
        io.Say("Welcome to the YNAB Seeder");
        io.Pause();
    }

    protected override void OnRunComplete(ICliWorkflowRun run, CliCommandOutcome[] outcomes)
    {
        io.Pause();
    }
}