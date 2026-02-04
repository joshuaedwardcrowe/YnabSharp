using KitCli.Commands.Abstractions.Handlers;
using KitCli.Commands.Abstractions.Outcomes;
using KitCli.Commands.Abstractions.Outcomes.Final;

namespace YnabSharp.Seeder.Step2.Prepare;

public class PrepareCliCommandHandler : ICliCommandHandler<PrepareCliCommand>
{
    public Task<CliCommandOutcome[]> Handle(PrepareCliCommand request, CancellationToken cancellationToken)
    {
        var outcomes = new CliCommandOutcome[]
        {
            new OutputCliCommandOutcome("Not chosen something to prepare, choose 'accounts' or 'transactions'.")
        };
        
        return Task.FromResult(outcomes);
    }
}