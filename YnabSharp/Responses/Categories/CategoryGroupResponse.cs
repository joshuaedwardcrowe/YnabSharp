using System.Text.Json.Serialization;
using YnabSharpa.Responses.Category;

namespace YnabSharpa.Responses.Categories;

public class CategoryGroupResponse
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("categories")]
    public required IEnumerable<CategoryResponse> Categories { get; set; }
}