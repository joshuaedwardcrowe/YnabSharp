using System.Text.Json.Serialization;

namespace YnabSharp.Responses.Transactions;

public class GetTransactionResponse
{
    [JsonPropertyName("transaction")]
    public required TransactionResponse Transaction { get; set; }
}