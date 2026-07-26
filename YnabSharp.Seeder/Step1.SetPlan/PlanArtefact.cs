using KitCli.Commands.Abstractions.Artefacts;
using YnabSharp.Connected;

namespace YnabSharp.Seeder.Step1.SetPlan;

public class PlanArtefact(ConnectedPlan plan)
    : ValuedCliCommandArtefact<ConnectedPlan>(nameof(ConnectedPlan), plan);