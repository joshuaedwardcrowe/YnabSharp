using System.Text.Json.Serialization;

namespace YnabSharpa.Responses.Transactions;

public record TransactionResponse : SplitTransactionResponse
{
    [JsonPropertyName("flag_color")]
    public FlagColor? FlagColor { get; set; }
    
    [JsonPropertyName("flag_name")]
    public string? FlagName { get; set; }
    
    [JsonPropertyName("subtransactions")]
    public IEnumerable<SplitTransactionResponse> SplitTransactions { get; set; } = [];
}