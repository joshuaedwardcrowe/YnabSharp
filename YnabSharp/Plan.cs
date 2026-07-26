using YnabSharp.Responses.Plans;

namespace YnabSharp;

public class Plan(PlanResponse planResponse)
{
    /// <summary>
    /// Unique identifier of the plan.
    /// </summary>
    public Guid Id => planResponse.Id;

    /// <summary>
    /// Name of the plan.
    /// </summary>
    public string Name => planResponse.Name;

    /// <summary>
    /// When the plan was created.
    /// </summary>
    public DateOnly Created => planResponse.FirstMonth;

    /// <summary>
    /// When the plan was last active.
    /// </summary>
    public DateOnly LastActive => planResponse.LastMonth;

    /// <summary>
    /// Get the years a plan has been active.
    /// </summary>
    public PlanYears GetYears()
    {
        var planActiveYearCount = LastActive.Year - Created.Year; // e.g. 3

        var planActiveYears = Enumerable
            .Range(Created.Year, planActiveYearCount)
            .ToList();

        var measurableYears = planActiveYears

            // Need to miss off first year as there's no prior year to compare with.
            .Skip(1)

            // Need to miss off last (current year) as it's incomplete.
            .Take(planActiveYearCount - 1)

            .ToList();

        return new PlanYears(planActiveYears, measurableYears);
    }
}