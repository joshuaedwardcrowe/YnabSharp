using System.Text.Json.Serialization;

namespace YnabSharp.Requests.Transactions;

public class TransactionRequest(string id, Guid accountId)
{
    [JsonPropertyName("id")] 
    public string Id { get; set; } = id;

    [JsonPropertyName("account_id")]
    public Guid AccountId { get; set; } = accountId;
}