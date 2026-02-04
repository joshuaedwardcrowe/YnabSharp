using KitCli.Commands.Abstractions.Handlers;
using KitCli.Commands.Abstractions.Outcomes;
using KitCli.Commands.Abstractions.Outcomes.Final;

namespace YnabSharp.Seeder.Step3.Seed.Step31.SeedCancel;

public class SeedCancelCliCommandHandler : ICliCommandHandler<SeedCancelCliCommand>
{
    public Task<CliCommandOutcome[]> Handle(SeedCancelCliCommand command, CancellationToken cancellationToken)
    {
        return Task.FromResult<CliCommandOutcome[]>(
            [
                new OutputCliCommandOutcome("Seeding cancelled.")
            ]
        );
    }
}