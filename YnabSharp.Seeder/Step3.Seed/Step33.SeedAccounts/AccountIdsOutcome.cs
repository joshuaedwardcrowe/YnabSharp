using KitCli.Commands.Abstractions.Outcomes;

namespace YnabSharp.Seeder.Step3.Seed.Step33.SeedAccounts;

public class AccountIdsOutcome(List<Guid> accountIds) : CliCommandOutcome(CliCommandOutcomeKind.Reusable)
{
    public List<Guid> AccountIds { get; } = accountIds;
}