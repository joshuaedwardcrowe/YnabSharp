using Bogus;
using KitCli.Abstractions.Tables;
using KitCli.Commands.Abstractions.Handlers;
using KitCli.Commands.Abstractions.Outcomes;
using KitCli.Commands.Abstractions.Outcomes.Final;
using KitCli.Commands.Abstractions.Outcomes.Reusable;
using YnabSharp.Responses.Accounts;
using YnabSharp.Sanitisers;

namespace YnabSharp.Seeder.Step2.Prepare.Step21.PrepareAccounts;

public class PrepareAccountsCliCommandHandler : ICliCommandHandler<PrepareAccountsCliCommand>
{
    public Task<CliCommandOutcome[]> Handle(PrepareAccountsCliCommand command, CancellationToken cancellationToken)
    {
        var faker = new Faker<AccountResponse>()
            .RuleFor(account => account.Name, setter => setter.Finance.AccountName())
            .RuleFor(account => account.OnBudget, setter => true)
            .RuleFor(account => account.ClearedBalance, setter =>
                MilliunitConverter.PoundsToMilliunit(setter.Finance.Amount()));

        var accountResponses = faker.Generate(command.Count);

        var accounts = accountResponses
            .Select(accountResponse => new Account(accountResponse))
            .ToList();

        var table = new Table
        {
            Columns = ["Name", "Cleared Balance"],
            Rows = accounts
                .Select(a => new List<object> { a.Name, a.ClearedBalance })
                .ToList()
        };

        var outcomes = new List<CliCommandOutcome>
        {
            new MessageCliCommandOutcome($"Prepared {accounts.Count} Fake Accounts"),
            new TableCliCommandOutcome(table),
            new AccountsOutcome(accounts)
        };

        return Task.FromResult(outcomes.ToArray());
    }
}