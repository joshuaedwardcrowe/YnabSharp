using YnabSharpa.Responses.Category;
using YnabSharpa.Sanitisers;

namespace YnabSharpa;

public class Category(CategoryResponse categoryResponse)
{
    public Guid Id => categoryResponse.Id;
    public string Name => categoryResponse.Name;
    public int GoalTarget => categoryResponse.GoalTarget;
    public DateOnly? GoalCreationMonth => categoryResponse.GoalCreationMonth;
    public DateOnly? GoalTargetMonth => categoryResponse.GoalTargetMonth;
    public decimal? GoalOverallFunded => MilliunitConverter.MilliunitToPounds(categoryResponse.GoalOverallFunded);
    public decimal? GoalOverallLeft => MilliunitConverter.MilliunitToPounds(categoryResponse.GoalOverallLeft);
    
    public bool HasGoal => GoalTarget > 0;
}