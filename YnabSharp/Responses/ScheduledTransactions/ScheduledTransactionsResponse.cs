using System.Text.Json.Serialization;

namespace YnabSharpa.Responses.ScheduledTransactions;

public class ScheduledTransactionsResponse
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("memo")]
    public string? Memo { get; set; }
    
    [JsonPropertyName("amount")]
    public int Amount { get; set; }
    
    [JsonPropertyName("date_next")]
    public DateTime NextOccurence { get; set; }
    
    [JsonPropertyName("account_id")]
    public Guid AccountId { get; set; }
}