using YnabSharpa.Responses.Categories;
using YnabSharpa.Sanitisers;

namespace YnabSharpa;

public class CategoryGroup(CategoryGroupResponse categoryGroupResponse)
{
    public string Name => categoryGroupResponse.Name;
    
    /// <summary>
    /// Money in these categories available to spend.
    /// </summary>
    public decimal Available
        => categoryGroupResponse.Categories.Sum(category => MilliunitConverter.Calculate(category.Available));
    
    public IEnumerable<Category> Categories
        => categoryGroupResponse.Categories.Select(category => new Category(category));
    
    public IEnumerable<Guid> GetCategoryIds()
        => categoryGroupResponse.Categories.Select(category => category.Id);
}