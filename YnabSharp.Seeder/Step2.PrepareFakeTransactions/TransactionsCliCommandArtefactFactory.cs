using KitCli.Commands.Abstractions.Artefacts;
using KitCli.Commands.Abstractions.Outcomes;

namespace YnabSharp.Seeder.Step2.PrepareFakeTransactions;

public class TransactionsCliCommandArtefactFactory : ICliCommandArtefactFactory
{
    public bool For(CliCommandOutcome outcome) => outcome is TransactionsOutcome;

    public CliCommandArtefact Create(CliCommandOutcome outcome)
    {
        var transactionsOutcome = (TransactionsOutcome)outcome;
        return new TransactionsArtefact(transactionsOutcome.Transactions);
    }
}