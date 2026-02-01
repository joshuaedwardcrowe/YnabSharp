using System.Text.Json.Serialization;

namespace YnabSharpa.Responses.Budgets;

public class GetBudgetsResponseData
{
    [JsonPropertyName("budgets")]
    public required IEnumerable<BudgetResponse> Budgets { get; set; }
}