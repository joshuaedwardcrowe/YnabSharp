using KitCli.Commands.Abstractions;
using KitCli.Commands.Abstractions.Artefacts;
using KitCli.Commands.Abstractions.Factories;
using KitCli.Instructions.Abstractions;
using YnabSharp.Connected;
using YnabSharp.Seeder.Step3.Seed.Step31.SeedCancel;
using YnabSharp.Seeder.Step3.Seed.Step32.SeedCheck;

namespace YnabSharp.Seeder.Step3.Seed;

public class SeedCliCommandFactory : ICliCommandFactory<SeedCliCommand>
{
    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        if (instruction.SubInstructionName == null)
        {
            // We haven't asked for confirmation yet
            return new SeedCheckCLiCommand();
        }

        if (instruction.SubInstructionName == SeedCliCommand.SubCommandNames.No)
        {
            return new SeedCancelCliCommand();
        }
        
        var budgetArtefact = artefacts
            .OfRequiredType<ConnectedBudget>();
        
        var accountsArtefact = artefacts
            .OfRequiredType<List<Account>>();
        
        var transactionsArtefact = artefacts
            .OfRequiredType<List<Transaction>>();
        
        return new SeedCliCommand(
            budgetArtefact.ArtefactValue,
            accountsArtefact.ArtefactValue,
            transactionsArtefact.ArtefactValue);
    }
}