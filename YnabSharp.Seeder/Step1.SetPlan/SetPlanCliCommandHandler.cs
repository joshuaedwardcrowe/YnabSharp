using KitCli.Commands.Abstractions.Handlers;
using KitCli.Commands.Abstractions.Outcomes;
using KitCli.Commands.Abstractions.Outcomes.Reusable;
using Microsoft.Extensions.Options;
using YnabSharp.Clients;
using YnabSharp.Http;

namespace YnabSharp.Seeder.Step1.SetPlan;

public class SetPlanCliCommandHandler : ICliCommandHandler<SetPlanCliCommand>
{
    private readonly YnabHttpClientBuilder _builder;
    private readonly YnabSharpSeederSettings _settings;

    public SetPlanCliCommandHandler(
        YnabHttpClientBuilder builder,
        IOptions<YnabSharpSeederSettings> settings)
    {
        _builder = builder;
        _settings = settings.Value;
    }

    public async Task<CliCommandOutcome[]> Handle(SetPlanCliCommand command, CancellationToken cancellationToken)
    {
        var builder = _builder.WithBearerToken(_settings.YnabApiKey);

        var client = new PlansClient(builder);

        var plan = await client.GetPlan(command.PlanName);

        var outputOutcome = plan == null
            ? new MessageCliCommandOutcome($"Plan '{command.PlanName}' not found.")
            : new MessageCliCommandOutcome($"Plan {command.PlanName} Identified: {plan.Id}");

        var outcomes = new List<CliCommandOutcome>
        {
            outputOutcome
        };

        if (plan != null)
        {
            outcomes.Add(new PlanOutcome(plan));
        }

        return outcomes.ToArray();
    }
}