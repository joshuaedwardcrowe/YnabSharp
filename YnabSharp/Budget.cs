using YnabSharp.Responses.Budgets;

namespace YnabSharp;

public class Budget(BudgetResponse budgetResponse)
{
    /// <summary>
    /// Unique identifier of the budget.
    /// </summary>
    public Guid Id => budgetResponse.Id;

    /// <summary>
    /// Name of the budget.
    /// </summary>
    public string Name => budgetResponse.Name;
    
    /// <summary>
    /// When the budget was created.
    /// </summary>
    public DateOnly Created => budgetResponse.FirstMonth;
    
    /// <summary>
    /// When the budget was last active.
    /// </summary>
    public DateOnly LastActive => budgetResponse.LastMonth;

    /// <summary>
    /// Get the years a budget has been active.
    /// </summary>
    public BudgetYears GetYears()
    {
        var budgetActiveYearCount = LastActive.Year - Created.Year; // e.g. 3

        var budgetActiveYears = Enumerable
            .Range(Created.Year, budgetActiveYearCount)
            .ToList();

        var measurableYears = budgetActiveYears
                
            // Need to miss off first year as there's no prior year to compare with.
            .Skip(1)

            // Need to miss off last (current year) as it's incomplete.
            .Take(budgetActiveYearCount - 1)
            
            .ToList();

        return new BudgetYears(budgetActiveYears, measurableYears);
    }
}