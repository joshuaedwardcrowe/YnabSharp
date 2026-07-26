using System.Text.Json.Serialization;

namespace YnabSharp.Responses.Plans;

public class GetPlanResponseData
{
    [JsonPropertyName("plan")]
    public required PlanResponse Plan { get; set; }
}