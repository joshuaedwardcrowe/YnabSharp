using KitCli.Commands.Abstractions.Handlers;
using KitCli.Commands.Abstractions.Outcomes;
using KitCli.Commands.Abstractions.Outcomes.Final;

namespace YnabSharp.Seeder.Step3.Seed;

public class SeedCliCommandHandler : ICliCommandHandler<SeedCliCommand>
{
    public async Task<CliCommandOutcome[]> Handle(SeedCliCommand command, CancellationToken cancellationToken)
    {
        await command.Budget.CreateTransactions(command.Transactions);
        
        return [
            new CliCommandOutputOutcome($"Seeded {command.Transactions.Count} Transactions.")
        ];
    }
}