using System.Text.Json.Serialization;

namespace YnabSharp.Responses.Categories;

public class GetCategoriesResponseData
{
    [JsonPropertyName("category_groups")]
    public required IEnumerable<CategoryGroupResponse> CategoryGroups { get; set; }
}