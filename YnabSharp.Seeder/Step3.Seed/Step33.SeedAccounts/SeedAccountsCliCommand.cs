using KitCli.Commands.Abstractions;
using YnabSharp.Connected;

namespace YnabSharp.Seeder.Step3.Seed.Step33.SeedAccounts;

public record SeedAccountsCliCommand(ConnectedPlan Plan, List<Account> Accounts) : CliCommand;