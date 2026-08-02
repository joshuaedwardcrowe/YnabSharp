using KitCli;
using KitCli.Abstractions.Io;
using KitCli.Commands.Abstractions.Outcomes;
using KitCli.Workflow.Abstractions;

namespace YnabSharp.Seeder;

public class YnabSharpSeederCliApp(ICliWorkflow workflow, ICliIo io) : CliApp(workflow, io)
{
    private readonly ICliIo _io = io;

    protected override void OnSessionStart()
    {
        _io.Say("Welcome to the YNAB Seeder");
        _io.Pause();
    }

    protected override void OnRunComplete(ICliWorkflowRun run, CliCommandOutcome[] outcomes)
    {
        _io.Pause();
    }
}