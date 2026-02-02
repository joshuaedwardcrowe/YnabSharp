using KitCli.Commands.Abstractions.Artefacts;
using KitCli.Commands.Abstractions.Outcomes;

namespace YnabSharp.Seeder.Step1.SetBudget;

public class BudgetCliCommandArtefactFactory : ICliCommandArtefactFactory
{
    public bool For(CliCommandOutcome outcome) => outcome is BudgetOutcome;

    public CliCommandArtefact Create(CliCommandOutcome outcome)
    {
        var budgetOutcome = (BudgetOutcome)outcome;
        return new BudgetArtefact(budgetOutcome.Budget);
    }
}