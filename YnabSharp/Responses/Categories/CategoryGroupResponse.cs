using System.Text.Json.Serialization;
using YnabSharp.Responses.Category;

namespace YnabSharp.Responses.Categories;

public class CategoryGroupResponse
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("categories")]
    public required IEnumerable<CategoryResponse> Categories { get; set; }
}