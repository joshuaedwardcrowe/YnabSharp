using Bogus;
using KitCli.Abstractions.Tables;
using KitCli.Commands.Abstractions.Handlers;
using KitCli.Commands.Abstractions.Outcomes;
using KitCli.Commands.Abstractions.Outcomes.Final;
using KitCli.Commands.Abstractions.Outcomes.Reusable;
using YnabSharp.Responses.Transactions;

namespace YnabSharp.Seeder.Step2.Prepare.Step22.PrepareTransactions;

public class PrepareTransactionsCliCommandHandler : ICliCommandHandler<PrepareTransactionsCliCommand>
{
    public Task<CliCommandOutcome[]> Handle(PrepareTransactionsCliCommand command, CancellationToken cancellationToken)
    {
        var faker = new Faker<TransactionResponse>()
            .RuleFor(t => t.Occured, f => f.Date.Past())
            .RuleFor(t => t.Amount, f => f.Random.Int(-100000, 100000))
            .RuleFor(t => t.Memo, f => f.Lorem.Sentence())
            .RuleFor(t => t.PayeeName, f => f.Person.FullName)
            .RuleFor(t => t.CategoryName, f => f.Lorem.Sentence())
            .RuleFor(t => t.AccountId, f => f.PickRandom(command.AccountIds));

        var transactionResponses = faker.Generate(command.Count);
        
        var transactions = transactionResponses
            .Select(transactionResponse => new Transaction(transactionResponse))
            .ToList();
        
        var table = new Table
        {
            Columns = ["Category Name", "Payee Name", "Amount"],
            Rows = transactionResponses
                .Select(a => new List<object> { a.CategoryName, a.PayeeName, a.Amount })
                .ToList()
        };

        var outcomes = new List<CliCommandOutcome>
        {
            new MessageCliCommandOutcome($"Prepared {transactions.Count} Fake Transactions"),
            new TableCliCommandOutcome(table),
            new TransactionsOutcome(transactions)
        };

        return Task.FromResult(outcomes.ToArray());
    }
}