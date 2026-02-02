using System.Text.Json.Serialization;

namespace YnabSharp.Responses.Budgets;

public class GetBudgetResponseData
{
    [JsonPropertyName("budget")]
    public required BudgetResponse Budget { get; set; }
}