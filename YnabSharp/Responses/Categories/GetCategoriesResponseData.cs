using System.Text.Json.Serialization;

namespace YnabSharpa.Responses.Categories;

public class GetCategoriesResponseData
{
    [JsonPropertyName("category_groups")]
    public required IEnumerable<CategoryGroupResponse> CategoryGroups { get; set; }
}