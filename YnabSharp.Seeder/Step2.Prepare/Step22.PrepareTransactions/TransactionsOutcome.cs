using KitCli.Commands.Abstractions.Outcomes;

namespace YnabSharp.Seeder.Step2.Prepare.Step22.PrepareTransactions;

public class TransactionsOutcome(List<Transaction> transactions) : CliCommandOutcome(CliCommandOutcomeKind.Reusable)
{
    public List<Transaction> Transactions { get; } = transactions;
}