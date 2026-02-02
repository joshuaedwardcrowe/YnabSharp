using KitCli.Commands.Abstractions;
using KitCli.Commands.Abstractions.Artefacts;
using KitCli.Commands.Abstractions.Factories;
using KitCli.Instructions.Abstractions;
using KitCli.Instructions.Arguments;
using YnabSharp.Connected;

namespace YnabSharp.Seeder.Step2.PrepareFakeTransactions;

public class PrepareFakeTransactionsCliCommandFactory : ICliCommandFactory<PrepareFakeTransactionsCliCommand>
{
    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        var budgetArtefact = artefacts
            .OfRequiredType<ConnectedBudget>();
        
        var countArgument = instruction
            .Arguments
            .OfRequiredType<int>(PrepareFakeTransactionsCliCommand.ArgumentNames.Count);
        
        return new PrepareFakeTransactionsCliCommand(
            countArgument.ArgumentValue);
    }
}