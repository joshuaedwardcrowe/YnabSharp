using KitCli.Commands.Abstractions.Handlers;
using KitCli.Commands.Abstractions.Outcomes;
using KitCli.Commands.Abstractions.Outcomes.Reusable;

namespace YnabSharp.Seeder.Step3.Seed;

public class SeedCliCommandHandler : ICliCommandHandler<SeedCliCommand>
{
    public Task<CliCommandOutcome[]> Handle(SeedCliCommand command, CancellationToken cancellationToken)
    {
        return Task.FromResult(new CliCommandOutcome[]
        {
            new MessageCliCommandOutcome("Please choose to seed accounts or transactions using the sub-commands.")
        });
    }
}