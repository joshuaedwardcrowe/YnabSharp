using KitCli.Commands.Abstractions;

namespace YnabSharp.Seeder.Step3.Seed;

public record SeedCliCommand: CliCommand
{
    public static class SubCommandNames
    {
        public const string Accounts = "accounts";
        public const string Transactions = "transactions";
        
        public static readonly List<string> All = [Accounts, Transactions];
    }
}