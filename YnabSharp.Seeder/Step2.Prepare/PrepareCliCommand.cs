using KitCli.Commands.Abstractions;

namespace YnabSharp.Seeder.Step2.Prepare;

public record PrepareCliCommand : CliCommand
{
    public static class SubCommandNames
    {
        public const string Accounts = "accounts";
        public const string Transactions = "transactions";
    }
}