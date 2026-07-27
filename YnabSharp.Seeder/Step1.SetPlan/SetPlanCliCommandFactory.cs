using KitCli.Commands.Abstractions;
using KitCli.Commands.Abstractions.Artefacts;
using KitCli.Commands.Abstractions.Factories;
using KitCli.Instructions.Abstractions;
using KitCli.Instructions.Arguments;

namespace YnabSharp.Seeder.Step1.SetPlan;

public class SetPlanCliCommandFactory : ICliCommandFactory<SetPlanCliCommand>
{
    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        var countArgument = instruction
            .Arguments
            .OfRequiredType<string>(SetPlanCliCommand.ArgumentNames.PlanName);

        return new SetPlanCliCommand(countArgument.ArgumentValue);
    }
}