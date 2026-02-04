using KitCli.Commands.Abstractions;
using YnabSharp.Connected;

namespace YnabSharp.Seeder.Step3.Seed.Step34.SeedTransactions;

public record SeedTransactionsCliCommand(ConnectedBudget Budget, List<Transaction> Transactions) : CliCommand;