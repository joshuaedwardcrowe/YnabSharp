using KitCli.Commands.Abstractions;
using KitCli.Commands.Abstractions.Artefacts;
using KitCli.Commands.Abstractions.Factories;
using KitCli.Instructions.Abstractions;
using YnabSharp.Connected;

namespace YnabSharp.Seeder.Step3.Seed;

public class SeedCliCommandFactory : ICliCommandFactory<SeedCliCommand>
{
    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        var budgetArtefact = artefacts
            .OfRequiredType<ConnectedBudget>();
        
        var transactionsArtefact = artefacts
            .OfRequiredType<List<Transaction>>();
        
        return new SeedCliCommand(
            budgetArtefact.ArtefactValue,
            transactionsArtefact.ArtefactValue);
    }
}