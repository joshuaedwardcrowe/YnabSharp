using KitCli.Commands.Abstractions;
using KitCli.Commands.Abstractions.Artefacts;
using KitCli.Commands.Abstractions.Factories;
using KitCli.Instructions.Abstractions;
using KitCli.Instructions.Arguments;

namespace YnabSharp.Seeder.Step2.Prepare.Step22.PrepareTransactions;

public class PrepareTransactionsCliCommandFactory : ICliCommandFactory<PrepareCliCommand>
{
    public bool CanCreateWhen(CliInstruction instruction, List<CliCommandArtefact> artefacts)
        => instruction.SubInstructionName == PrepareCliCommand.SubCommandNames.Transactions;

    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        var accountsArtefact = artefacts.OfRequiredType<List<Account>>();
        
        var countArgument = instruction
            .Arguments
            .OfRequiredType<int>(PrepareTransactionsCliCommand.ArgumentNames.Count);
        
        return new PrepareTransactionsCliCommand(
            accountsArtefact.ArtefactValue,
            countArgument.ArgumentValue);
    }
}