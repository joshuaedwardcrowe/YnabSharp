using KitCli.Commands.Abstractions;
using YnabSharp.Connected;

namespace YnabSharp.Seeder.Step3.Seed;

public record SeedCliCommand(ConnectedBudget Budget, List<Transaction> Transactions) : CliCommand;