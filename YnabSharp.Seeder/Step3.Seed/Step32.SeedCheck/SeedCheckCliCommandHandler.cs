using KitCli.Commands.Abstractions.Handlers;
using KitCli.Commands.Abstractions.Outcomes;
using KitCli.Commands.Abstractions.Outcomes.Reusable;

namespace YnabSharp.Seeder.Step3.Seed.Step32.SeedCheck;

public class SeedCheckCliCommandHandler : ICliCommandHandler<SeedCheckCLiCommand>
{
    public Task<CliCommandOutcome[]> Handle(SeedCheckCLiCommand command, CancellationToken cancellationToken)
    {
        return Task.FromResult<CliCommandOutcome[]>(
            [
                new MessageCliCommandOutcome("Are you sure?"),
                new MessageCliCommandOutcome("This will seed the prepared accounts and transactions"),
                new MessageCliCommandOutcome("into the identified budget. This cannot be ondone.")
            ]
        );
    }
}