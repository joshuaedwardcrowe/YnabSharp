using KitCli.Commands.Abstractions;
using YnabSharp.Connected;

namespace YnabSharp.Seeder.Step3.Seed.Step33.SeedAccounts;

public record SeedAccountsCliCommand(ConnectedBudget Budget, List<Account> Accounts) : CliCommand;