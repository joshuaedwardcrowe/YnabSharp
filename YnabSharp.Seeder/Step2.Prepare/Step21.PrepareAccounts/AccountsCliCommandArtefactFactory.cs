using KitCli.Commands.Abstractions.Artefacts;
using KitCli.Commands.Abstractions.Outcomes;

namespace YnabSharp.Seeder.Step2.Prepare.Step21.PrepareAccounts;

public class AccountsCliCommandArtefactFactory : ICliCommandArtefactFactory
{
    public bool For(CliCommandOutcome outcome) => outcome is AccountsOutcome;

    public CliCommandArtefact Create(CliCommandOutcome outcome)
    {
        var accountsOutcome = (AccountsOutcome)outcome;
        return new AccountsArtefact(accountsOutcome.Accounts);
    }
}