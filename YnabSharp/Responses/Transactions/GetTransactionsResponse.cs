using System.Text.Json.Serialization;

namespace YnabSharp.Responses.Transactions;

public class GetTransactionsResponse
{
    [JsonPropertyName("transactions")]
    public required IEnumerable<TransactionResponse> Transactions { get; set; }
}