using System.Text.Json.Serialization;

namespace YnabSharp.Responses.Budgets;

public class GetBudgetsResponseData
{
    [JsonPropertyName("budgets")]
    public required IEnumerable<BudgetResponse> Budgets { get; set; }
}