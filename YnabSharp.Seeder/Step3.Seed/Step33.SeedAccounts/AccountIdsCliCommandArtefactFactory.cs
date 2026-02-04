using KitCli.Commands.Abstractions.Artefacts;
using KitCli.Commands.Abstractions.Outcomes;

namespace YnabSharp.Seeder.Step3.Seed.Step33.SeedAccounts;

public class AccountIdsCliCommandArtefactFactory : ICliCommandArtefactFactory
{
    public bool For(CliCommandOutcome outcome) => outcome is AccountIdsOutcome;

    public CliCommandArtefact Create(CliCommandOutcome outcome)
    {
        var accountIdsOutcome = (AccountIdsOutcome)outcome;
        return new AccountIdsArtefact(accountIdsOutcome.AccountIds);
    }
}