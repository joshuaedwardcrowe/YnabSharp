using System.Text.Json.Serialization;

namespace YnabSharp.Requests.Transactions;

public class UpdateTransactionRequest(IEnumerable<TransactionRequest> transactions)
{
    [JsonPropertyName("transactions")]
    public IEnumerable<TransactionRequest> Transactions { get; set; } = transactions;
}