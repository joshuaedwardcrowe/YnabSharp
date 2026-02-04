using KitCli.Abstractions.Io;
using KitCli.Commands.Abstractions.Handlers;
using KitCli.Commands.Abstractions.Outcomes;
using KitCli.Commands.Abstractions.Outcomes.Final;

namespace YnabSharp.Seeder.Step3.Seed;

public class SeedCliCommandHandler(ICliIo cliIo) : ICliCommandHandler<SeedCliCommand>
{
    public async Task<CliCommandOutcome[]> Handle(SeedCliCommand command, CancellationToken cancellationToken)
    {
        foreach (var account in command.Accounts)
        {
            var seededAccount = await command.Budget.CreateAccount(account);
            cliIo.Say($"Seeded Account: {seededAccount.Id}");
        }
        
        var seededTransactions = await command.Budget.CreateTransactions(command.Transactions);
        foreach (var transaction in seededTransactions)
        {
            cliIo.Say($"Seeded Transaction: {transaction.Id}");
        }
        
        return [
            new OutputCliCommandOutcome($"Seeded {command.Transactions.Count} Transactions.")
        ];
    }
}