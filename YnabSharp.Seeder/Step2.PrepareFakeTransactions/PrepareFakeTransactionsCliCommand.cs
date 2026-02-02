using KitCli.Commands.Abstractions;
using YnabSharp.Connected;

namespace YnabSharp.Seeder.Step2.PrepareFakeTransactions;

public record PrepareFakeTransactionsCliCommand(ConnectedBudget Budget, int Count) : CliCommand
{
    public static class ArgumentNames
    {
        public const string Count = "count";
    }
}