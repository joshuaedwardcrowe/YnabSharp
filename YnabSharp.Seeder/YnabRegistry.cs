using KitCli.Abstractions;
using KitCli.Commands.Abstractions.Artefacts;
using Microsoft.Extensions.DependencyInjection;
using YnabSharp.Extensions;
using YnabSharp.Seeder.Step1.SetBudget;
using YnabSharp.Seeder.Step2.PrepareFakeTransactions;

namespace YnabSharp.Seeder;

public class YnabRegistry : ICliAppBuilderRegistry
{
    public void Register(IServiceCollection services)
    {
        services.AddYnab();

        services.AddSingleton<ICliCommandArtefactFactory, BudgetCliCommandArtefactFactory>();
        services.AddSingleton<ICliCommandArtefactFactory, TransactionsCliCommandArtefactFactory>();
    }
}