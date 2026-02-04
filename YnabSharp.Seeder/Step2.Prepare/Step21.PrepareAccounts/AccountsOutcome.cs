using KitCli.Commands.Abstractions.Outcomes;

namespace YnabSharp.Seeder.Step2.Prepare.Step21.PrepareAccounts;

public class AccountsOutcome(List<Account> accounts) : CliCommandOutcome(CliCommandOutcomeKind.Reusable)
{
    public List<Account> Accounts { get; } = accounts;
}