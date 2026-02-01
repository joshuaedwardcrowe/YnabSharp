using System.Text.Json.Serialization;

namespace YnabSharpa.Responses.Accounts;

public record AccountResponse
{
    [JsonPropertyName("id")]
    public required Guid Id { get; set; }
    
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("type")]
    public required AccountType Type { get; set; }
    
    [JsonPropertyName("cleared_balance")]
    public required int ClearedBalance { get; set; }

    [JsonPropertyName("closed")]
    public required bool Closed { get; set; }

    [JsonPropertyName("on_budget")]
    public required bool OnBudget { get; set; }
}