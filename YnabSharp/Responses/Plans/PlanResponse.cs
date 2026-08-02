using System.Text.Json.Serialization;

namespace YnabSharp.Responses.Plans;

public class PlanResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("first_month")]
    public DateOnly? FirstMonth { get; set; }

    [JsonPropertyName("last_month")]
    public DateOnly? LastMonth { get; set; }
}