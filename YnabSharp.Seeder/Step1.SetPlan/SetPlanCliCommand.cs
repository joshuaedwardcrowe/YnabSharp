using KitCli.Commands.Abstractions;

namespace YnabSharp.Seeder.Step1.SetPlan;

public record SetPlanCliCommand(string PlanName) : CliCommand
{
    public static class ArgumentNames
    {
        public const string PlanName = "planName";
    }
}