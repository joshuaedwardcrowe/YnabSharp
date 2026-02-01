using YnabSharpa.Http;
using YnabSharpa.Responses.Categories;

namespace YnabSharpa.Clients;

public class CategoryClient(YnabHttpClientBuilder builder, string ynabBudgetApiPath) : YnabApiClient
{
    public async Task<IEnumerable<CategoryGroup>> GetAll()
    {
        var response = await Get<GetCategoriesResponseData>(string.Empty);
        return response.Data.CategoryGroups.Select(categoryGroup => new CategoryGroup(categoryGroup));
    }
    
    protected override HttpClient GetHttpClient() => builder.Build(ynabBudgetApiPath, YnabApiPath.Categories);
}