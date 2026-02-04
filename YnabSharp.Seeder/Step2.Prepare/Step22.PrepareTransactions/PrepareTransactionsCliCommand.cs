using KitCli.Commands.Abstractions;

namespace YnabSharp.Seeder.Step2.Prepare.Step22.PrepareTransactions;

public record PrepareTransactionsCliCommand(List<Account> Accounts, int Count) : CliCommand
{
    public static class ArgumentNames
    {
        public const string Count = "count";
    }
}