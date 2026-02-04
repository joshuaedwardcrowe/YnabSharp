using KitCli.Commands.Abstractions;

namespace YnabSharp.Seeder.Step2.Prepare.Step21.PrepareAccounts;

public record PrepareAccountsCliCommand(int Count) : CliCommand
{
    public static class ArgumentNames
    {
        public const string Count = "count";
    }
}