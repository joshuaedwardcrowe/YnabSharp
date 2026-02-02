using KitCli.Commands.Abstractions.Outcomes;
using YnabSharp.Connected;

namespace YnabSharp.Seeder.Step1.SetBudget;

public class BudgetOutcome(ConnectedBudget budget) : CliCommandOutcome(CliCommandOutcomeKind.Reusable)
{
    public ConnectedBudget Budget { get; } = budget;
}