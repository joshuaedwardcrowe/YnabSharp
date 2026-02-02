using KitCli.Commands.Abstractions.Artefacts;

namespace YnabSharp.Seeder.Step2.PrepareFakeTransactions;

public class TransactionsArtefact(List<Transaction> transactions) 
    : ValuedCliCommandArtefact<List<Transaction>>(nameof(Transaction), transactions);