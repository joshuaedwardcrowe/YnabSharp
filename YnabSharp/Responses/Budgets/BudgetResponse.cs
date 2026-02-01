using System.Text.Json.Serialization;

namespace YnabSharp.Responses.Budgets;

public class BudgetResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("first_month")]
    public DateOnly FirstMonth { get; set; }
    
    [JsonPropertyName("last_month")]
    public DateOnly LastMonth { get; set; }
}