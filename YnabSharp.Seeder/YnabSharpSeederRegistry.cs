using KitCli.Abstractions;
using KitCli.Commands.Abstractions.Extensions;
using Microsoft.Extensions.DependencyInjection;
using YnabSharp.Seeder.Step1.SetPlan;

namespace YnabSharp.Seeder;

public class YnabSharpSeederRegistry : ICliAppBuilderRegistry
{
    public void Register(IServiceCollection services)
    {
        services.AddCommandsFromAssembly(typeof(SetPlanCliCommand).Assembly);
    }
}