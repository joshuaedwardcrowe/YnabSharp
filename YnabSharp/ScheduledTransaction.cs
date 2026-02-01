using YnabSharp.Responses.ScheduledTransactions;
using YnabSharp.Sanitisers;

namespace YnabSharp;

public class ScheduledTransaction(ScheduledTransactionsResponse scheduledTransactionsResponse)
{
    public string Id => scheduledTransactionsResponse.Id;
    public decimal Amount => MilliunitConverter.Calculate(scheduledTransactionsResponse.Amount);
    public DateTime NextOccurence => scheduledTransactionsResponse.NextOccurence;
    public Guid AccountId => scheduledTransactionsResponse.AccountId;
}