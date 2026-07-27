using KitCli.Commands.Abstractions.Outcomes;
using YnabSharp.Connected;

namespace YnabSharp.Seeder.Step1.SetPlan;

public class PlanOutcome(ConnectedPlan plan) : CliCommandOutcome(CliCommandOutcomeKind.Reusable)
{
    public ConnectedPlan Plan { get; } = plan;
}