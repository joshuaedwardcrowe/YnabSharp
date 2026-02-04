using KitCli.Commands.Abstractions;
using KitCli.Commands.Abstractions.Artefacts;
using KitCli.Commands.Abstractions.Factories;
using KitCli.Instructions.Abstractions;

namespace YnabSharp.Seeder.Step3.Seed;

public class SeedCliCommandFactory : ICliCommandFactory<SeedCliCommand>
{
    public bool CanCreateWhen(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        return instruction.SubInstructionName is null || !SeedCliCommand.SubCommandNames.All.Contains(instruction.SubInstructionName);
    }

    public CliCommand Create(CliInstruction instruction, List<CliCommandArtefact> artefacts)
    {
        return new SeedCliCommand();
    }
}