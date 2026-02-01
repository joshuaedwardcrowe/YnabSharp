namespace YnabSharpa;

public class CategoryGoal
{
    public int GoalTarget { get; set; }
    public DateOnly? GoalCreationMonth { get; set; }
    public DateOnly? GoalTargetMonth { get; set; }
    public decimal? GoalOverallFunded { get; set; }
    public decimal? GoalOverallLeft { get; set; }
}