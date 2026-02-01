using System.Text.Json.Serialization;

namespace YnabSharpa.Requests.ScheduledTransactions;

public class ScheduledTransactionRequest(string id, Guid accountId)
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = id;

    [JsonPropertyName("account_id")]
    public Guid AccountId { get; set; } = accountId;
}