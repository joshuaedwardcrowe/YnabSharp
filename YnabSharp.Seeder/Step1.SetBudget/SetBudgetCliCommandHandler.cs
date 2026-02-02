using KitCli.Commands.Abstractions.Handlers;
using KitCli.Commands.Abstractions.Outcomes;
using KitCli.Commands.Abstractions.Outcomes.Final;
using Microsoft.Extensions.Options;
using YnabSharp.Clients;
using YnabSharp.Http;

namespace YnabSharp.Seeder.Step1.SetBudget;

public class SetBudgetCliCommandHandler : ICliCommandHandler<SetBudgetCliCommand>
{
    private readonly YnabHttpClientBuilder _builder;
    private readonly YnabSharpSeederSettings _settings;

    public SetBudgetCliCommandHandler(
        YnabHttpClientBuilder builder,
        IOptions<YnabSharpSeederSettings> settings)
    {
        _builder = builder;
        _settings = settings.Value;
    }

    public async Task<CliCommandOutcome[]> Handle(SetBudgetCliCommand command, CancellationToken cancellationToken)
    {
        var builder = _builder.WithBearerToken(_settings.YnabApiKey);
        
        var client = new BudgetsClient(builder);

        var budget = await client.GetBudget(command.BudgetName);
        
        var outputOutcome = budget == null 
            ? new CliCommandOutputOutcome($"Budget '{command.BudgetName}' not found.") 
            : new CliCommandOutputOutcome($"Budget {command.BudgetName} Identified: {budget.Id}");
        
        var outcomes = new List<CliCommandOutcome>
        {
            outputOutcome
        };
        
        if (budget != null)
        {
            outcomes.Add(new BudgetOutcome(budget));
        }
        
        return outcomes.ToArray();
    }
}