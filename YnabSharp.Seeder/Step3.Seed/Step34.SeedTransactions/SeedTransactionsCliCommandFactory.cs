using KitCli.Commands.Abstractions;
using KitCli.Commands.Abstractions.Artefacts;
using KitCli.Commands.Abstractions.Factories;
using KitCli.Instructions.Abstractions;
using YnabSharp.Connected;

namespace YnabSharp.Seeder.Step3.Seed.Step34.SeedTransactions;

public class SeedTransactionsCliCommandFactory : ICliCommandFactory<SeedCliCommand>
{
    public bool CanCreateWhen(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        return instruction.SubInstructionName == SeedCliCommand.SubCommandNames.Transactions;
    }

    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        var budgetArtefact = artefacts
            .OfRequiredType<ConnectedBudget>();
        
        var transactionsArtefact = artefacts
            .OfRequiredType<List<Transaction>>();
        
        return new SeedTransactionsCliCommand(
            budgetArtefact.ArtefactValue,
            transactionsArtefact.ArtefactValue);
    }
}