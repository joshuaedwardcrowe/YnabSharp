using Bogus;
using KitCli.Commands.Abstractions.Handlers;
using KitCli.Commands.Abstractions.Outcomes;
using KitCli.Commands.Abstractions.Outcomes.Final;
using YnabSharp.Responses.Transactions;

namespace YnabSharp.Seeder.Step2.PrepareFakeTransactions;

public class PrepareFakeTransactionsCliCommandHandler : ICliCommandHandler<PrepareFakeTransactionsCliCommand>
{
    public async Task<CliCommandOutcome[]> Handle(PrepareFakeTransactionsCliCommand command, CancellationToken cancellationToken)
    {
        // TODO: Move this to a separate command.
        var accounts = await command.Budget.GetAccounts();
        
        var accountIds = accounts.Select(a => a.Id).ToArray();

        var faker = new Faker<TransactionResponse>()
            .RuleFor(t => t.Id, f => f.Random.Guid().ToString())
            .RuleFor(t => t.Amount, f => f.Random.Int(-100000, 100000))
            .RuleFor(t => t.Memo, f => f.Lorem.Sentence())
            .RuleFor(t => t.PayeeName, f => f.Person.FullName)
            .RuleFor(t => t.CategoryName, f => f.Lorem.Sentence())
            .RuleFor(t => t.AccountId, f => f.PickRandom(accountIds));

        var transactionResponses = faker.Generate(command.Count);
        
        var transactions = transactionResponses
            .Select(transactionResponse => new Transaction(transactionResponse))
            .ToList();

        return
        [
            new CliCommandOutputOutcome($"Prepared {transactions.Count} Fake Transactions"),
            new TransactionsOutcome(transactions)
        ];
    }
}