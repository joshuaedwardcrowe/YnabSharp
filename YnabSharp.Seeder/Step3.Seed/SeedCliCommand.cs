using KitCli.Commands.Abstractions;
using YnabSharp.Connected;

namespace YnabSharp.Seeder.Step3.Seed;

public record SeedCliCommand(
    ConnectedBudget Budget,
    List<Account> Accounts,
    List<Transaction> Transactions) : CliCommand
{
    public static class SubCommandNames
    {
        public const string Yes = "yes";
        public const string No = "no";
    }
}