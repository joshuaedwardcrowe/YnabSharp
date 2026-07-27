using System.Text.Json.Serialization;

namespace YnabSharp.Responses.Plans;

public class GetPlansResponseData
{
    [JsonPropertyName("plans")]
    public required IEnumerable<PlanResponse> Plans { get; set; }
}