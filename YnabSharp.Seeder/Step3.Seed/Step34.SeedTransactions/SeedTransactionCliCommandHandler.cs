using KitCli.Commands.Abstractions.Handlers;
using KitCli.Commands.Abstractions.Outcomes;
using KitCli.Commands.Abstractions.Outcomes.Final;

namespace YnabSharp.Seeder.Step3.Seed.Step34.SeedTransactions;

public class SeedTransactionCliCommandHandler : ICliCommandHandler<SeedTransactionsCliCommand>
{
    public async Task<CliCommandOutcome[]> Handle(SeedTransactionsCliCommand command, CancellationToken cancellationToken)
    {
        var transactions = await command.Budget.CreateTransactions(command.Transactions);
        
        return transactions
            .Select(transaction => new OutputCliCommandOutcome($"Seeded Transaction: {transaction.Id}"))
            .ToArray<CliCommandOutcome>();
    }
}