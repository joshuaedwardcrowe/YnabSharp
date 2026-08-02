using YnabSharp.Responses.ScheduledTransactions;
using YnabSharp.Sanitisers;

namespace YnabSharp;

public class ScheduledTransaction(ScheduledTransactionsResponse scheduledTransactionsResponse)
{
    public string Id => scheduledTransactionsResponse.Id;
    public decimal Amount => MilliunitConverter.Calculate(scheduledTransactionsResponse.Amount);
    public DateOnly DateFirst => scheduledTransactionsResponse.DateFirst;
    public DateTime NextOccurence => scheduledTransactionsResponse.NextOccurence;
    public ScheduledTransactionFrequency Frequency => scheduledTransactionsResponse.Frequency;
    public Guid AccountId => scheduledTransactionsResponse.AccountId;
    public bool Deleted => scheduledTransactionsResponse.Deleted;
}