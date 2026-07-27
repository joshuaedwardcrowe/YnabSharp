using KitCli.Commands.Abstractions.Artefacts;
using KitCli.Commands.Abstractions.Outcomes;

namespace YnabSharp.Seeder.Step1.SetPlan;

public class PlanCliCommandArtefactFactory : ICliCommandArtefactFactory
{
    public bool For(CliCommandOutcome outcome) => outcome is PlanOutcome;

    public CliCommandArtefact Create(CliCommandOutcome outcome)
    {
        var planOutcome = (PlanOutcome)outcome;
        return new PlanArtefact(planOutcome.Plan);
    }
}