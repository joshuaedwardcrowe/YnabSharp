using KitCli.Commands.Abstractions;
using KitCli.Commands.Abstractions.Artefacts;
using KitCli.Commands.Abstractions.Factories;
using KitCli.Instructions.Abstractions;
using YnabSharp.Connected;

namespace YnabSharp.Seeder.Step3.Seed.Step33.SeedAccounts;

public class SeedAccountsCliCommandFactory : ICliCommandFactory<SeedCliCommand>
{
    public bool CanCreateWhen(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        return instruction.SubInstructionName == SeedCliCommand.SubCommandNames.Accounts;
    }

    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        var budgetArtefact = artefacts
            .OfRequiredType<ConnectedBudget>();
        
        var accountsArtefact = artefacts
            .OfRequiredType<List<Account>>();
        
        return new SeedAccountsCliCommand(
            budgetArtefact.ArtefactValue,
            accountsArtefact.ArtefactValue);
    }
}