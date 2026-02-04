using KitCli.Abstractions.Io;
using KitCli.Commands.Abstractions.Handlers;
using KitCli.Commands.Abstractions.Outcomes;
using KitCli.Commands.Abstractions.Outcomes.Reusable;
using YnabSharp.Connected;

namespace YnabSharp.Seeder.Step3.Seed.Step33.SeedAccounts;

public class SeedAccountsCliCommandHandler(ICliIo cliIo) : ICliCommandHandler<SeedAccountsCliCommand>
{
    public async Task<CliCommandOutcome[]> Handle(SeedAccountsCliCommand command, CancellationToken cancellationToken)
    {
        var createAccountTasks = command.Accounts.Select(account => CreateAccount(command.Budget, account));
        
        var accountIds = await Task.WhenAll(createAccountTasks);

        return [
            new MessageCliCommandOutcome($"Seeded {command.Accounts.Count} Accounts."),
            new AccountIdsOutcome(accountIds.ToList())
        ];
    }

    private async Task<Guid> CreateAccount(ConnectedBudget budget, Account account)
    {
        var seededAccount = await budget.CreateAccount(account);
        cliIo.Say($"Seeded Account: {seededAccount.Id}");
        return seededAccount.Id;
    }
}