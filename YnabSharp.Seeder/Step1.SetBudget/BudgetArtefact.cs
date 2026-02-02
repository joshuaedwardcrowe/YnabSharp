using KitCli.Commands.Abstractions.Artefacts;
using YnabSharp.Connected;

namespace YnabSharp.Seeder.Step1.SetBudget;

public class BudgetArtefact(ConnectedBudget budget) 
    : ValuedCliCommandArtefact<ConnectedBudget>(nameof(ConnectedBudget), budget);