using KitCli.Abstractions;
using KitCli.Commands.Abstractions.Artefacts;
using Microsoft.Extensions.DependencyInjection;
using YnabSharp.Extensions;
using YnabSharp.Seeder.Step1.SetBudget;
using YnabSharp.Seeder.Step2.Prepare.Step21.PrepareAccounts;
using YnabSharp.Seeder.Step2.Prepare.Step22.PrepareTransactions;
using YnabSharp.Seeder.Step3.Seed.Step33.SeedAccounts;

namespace YnabSharp.Seeder;

public class YnabRegistry : ICliAppBuilderRegistry
{
    public void Register(IServiceCollection services)
    {
        services.AddYnab();
        
        RegisterArtefactFactories(services);
    }

    private void RegisterArtefactFactories(IServiceCollection services)
    {
        services.AddSingleton<ICliCommandArtefactFactory, BudgetCliCommandArtefactFactory>();
        services.AddSingleton<ICliCommandArtefactFactory, AccountsCliCommandArtefactFactory>();
        services.AddSingleton<ICliCommandArtefactFactory, AccountIdsCliCommandArtefactFactory>();
        services.AddSingleton<ICliCommandArtefactFactory, TransactionsCliCommandArtefactFactory>();       
    }
}