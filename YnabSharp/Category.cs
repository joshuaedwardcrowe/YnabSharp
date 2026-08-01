using YnabSharp.Responses.Category;
using YnabSharp.Sanitisers;

namespace YnabSharp;

public class Category(CategoryResponse categoryResponse)
{
    public Guid Id => categoryResponse.Id;
    public Guid CategoryGroupId => categoryResponse.CategoryGroupId;
    public string Name => categoryResponse.Name;
    public bool Hidden => categoryResponse.Hidden;
    public bool Internal => categoryResponse.Internal;
    public bool Deleted => categoryResponse.Deleted;
    public decimal Activity => MilliunitConverter.Calculate(categoryResponse.Activity);
    public int? GoalTarget => categoryResponse.GoalTarget;
    public DateOnly? GoalCreationMonth => categoryResponse.GoalCreationMonth;
    public DateOnly? GoalTargetMonth => categoryResponse.GoalTargetMonth;
    public decimal? GoalOverallFunded => MilliunitConverter.MilliunitToPounds(categoryResponse.GoalOverallFunded);
    public decimal? GoalOverallLeft => MilliunitConverter.MilliunitToPounds(categoryResponse.GoalOverallLeft);
    
    public bool HasGoal => GoalTarget > 0;
}