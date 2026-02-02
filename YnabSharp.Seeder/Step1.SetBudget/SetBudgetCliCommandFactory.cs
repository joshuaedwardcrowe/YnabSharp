using KitCli.Commands.Abstractions;
using KitCli.Commands.Abstractions.Artefacts;
using KitCli.Commands.Abstractions.Factories;
using KitCli.Instructions.Abstractions;
using KitCli.Instructions.Arguments;

namespace YnabSharp.Seeder.Step1.SetBudget;

public class SetBudgetCliCommandFactory : ICliCommandFactory<SetBudgetCliCommand>
{
    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        var countArgument = instruction
            .Arguments
            .OfRequiredType<string>(SetBudgetCliCommand.ArgumentNames.BudgetName);
        
        return new SetBudgetCliCommand(countArgument.ArgumentValue);
    }
}