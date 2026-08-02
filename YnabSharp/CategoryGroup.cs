using YnabSharp.Responses.Categories;
using YnabSharp.Sanitisers;

namespace YnabSharp;

public class CategoryGroup(CategoryGroupResponse categoryGroupResponse)
{
    public Guid Id => categoryGroupResponse.Id;
    public string Name => categoryGroupResponse.Name;
    public bool Hidden => categoryGroupResponse.Hidden;
    public bool Internal => categoryGroupResponse.Internal;
    public bool Deleted => categoryGroupResponse.Deleted;
    
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