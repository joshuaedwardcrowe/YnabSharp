using KitCli.Abstractions;
using KitCli.Commands.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using YnabSharp.Seeder.Step1.SetBudget;

namespace YnabSharp.Seeder;

public class YnabSharpSeederRegistry : ICliAppBuilderRegistry
{
    public void Register(IServiceCollection services)
    {
        services.AddCommandsFromAssembly(typeof(SetBudgetCliCommand).Assembly);
    }
}