using System.Text.Json.Serialization;

namespace YnabSharp.Requests.ScheduledTransactions;

public class UpdateScheduledTransactionRequest(ScheduledTransactionRequest scheduledTransactionRequest)
{
    [JsonPropertyName("scheduled_transaction")]
    public ScheduledTransactionRequest ScheduledTransaction { get; set; } = scheduledTransactionRequest;
}