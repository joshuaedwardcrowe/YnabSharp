using System.Text.Json.Serialization;

namespace YnabSharpa.Requests.ScheduledTransactions;

public class UpdateScheduledTransactionRequest(ScheduledTransactionRequest scheduledTransactionRequest)
{
    [JsonPropertyName("scheduled_transaction")]
    public ScheduledTransactionRequest ScheduledTransaction { get; set; } = scheduledTransactionRequest;
}