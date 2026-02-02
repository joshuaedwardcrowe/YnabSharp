using KitCli.Commands.Abstractions;

namespace YnabSharp.Seeder.Step1.SetBudget;

public record SetBudgetCliCommand(string BudgetName) : CliCommand
{
    public static class ArgumentNames
    {
        public const string BudgetName = "budgetName";
    }
}