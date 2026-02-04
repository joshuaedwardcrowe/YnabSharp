using KitCli.Commands.Abstractions;
using KitCli.Commands.Abstractions.Artefacts;
using KitCli.Commands.Abstractions.Factories;
using KitCli.Instructions.Abstractions;
using KitCli.Instructions.Arguments;

namespace YnabSharp.Seeder.Step2.Prepare.Step21.PrepareAccounts;

public class PrepareAccountsCliCommandFactory : ICliCommandFactory<PrepareCliCommand>
{
    public bool CanCreateWhen(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        return instruction.SubInstructionName == PrepareCliCommand.SubCommandNames.Accounts;
    }

    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        var countArgument = instruction
            .Arguments
            .OfRequiredType<int>(PrepareAccountsCliCommand.ArgumentNames.Count);
        
        return new PrepareAccountsCliCommand(
            countArgument.ArgumentValue);
    }
}