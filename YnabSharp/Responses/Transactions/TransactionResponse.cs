using System.Text.Json.Serialization;

namespace YnabSharp.Responses.Transactions;

public record TransactionResponse : SplitTransactionResponse
{
    [JsonPropertyName("flag_color")]
    public FlagColor? FlagColor { get; set; }

    [JsonPropertyName("flag_name")]
    public string? FlagName { get; set; }

    [JsonPropertyName("approved")]
    public required bool Approved { get; set; }

    [JsonPropertyName("cleared")]
    public required TransactionClearedStatus Cleared { get; set; }

    [JsonPropertyName("deleted")]
    public required bool Deleted { get; set; }

    [JsonPropertyName("account_name")]
    public required string AccountName { get; set; }

    [JsonPropertyName("subtransactions")]
    public IEnumerable<SplitTransactionResponse> SplitTransactions { get; set; } = [];
}