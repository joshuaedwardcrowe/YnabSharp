using System.Text.Json.Serialization;
using YnabSharp.Responses.Category;

namespace YnabSharp.Responses.Categories;

public class CategoryGroupResponse
{
    [JsonPropertyName("id")]
    public required Guid Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("hidden")]
    public required bool Hidden { get; set; }

    [JsonPropertyName("internal")]
    public required bool Internal { get; set; }

    [JsonPropertyName("deleted")]
    public required bool Deleted { get; set; }

    [JsonPropertyName("categories")]
    public required IEnumerable<CategoryResponse> Categories { get; set; }
}