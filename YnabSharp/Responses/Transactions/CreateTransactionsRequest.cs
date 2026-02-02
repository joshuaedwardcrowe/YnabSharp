using System.Text.Json.Serialization;
using YnabSharp.Requests.Transactions;

namespace YnabSharp.Responses.Transactions;

public class CreateTransactionsRequest
{
    [JsonPropertyName("transactions")]
    public IEnumerable<TransactionRequest> Transactions { get; set; }
    
    public CreateTransactionsRequest(IEnumerable<TransactionRequest> transactions)
    {
        Transactions = transactions;
    }
}