using KitCli.Commands.Abstractions.Artefacts;

namespace YnabSharp.Seeder.Step2.Prepare.Step22.PrepareTransactions;

public class TransactionsArtefact(List<Transaction> transactions) 
    : ValuedCliCommandArtefact<List<Transaction>>(nameof(Transaction), transactions);