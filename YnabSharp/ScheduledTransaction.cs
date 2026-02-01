using YnabSharpa.Responses.ScheduledTransactions;
using YnabSharpa.Sanitisers;

namespace YnabSharpa;

public class ScheduledTransaction(ScheduledTransactionsResponse scheduledTransactionsResponse)
{
    public string Id => scheduledTransactionsResponse.Id;
    public decimal Amount => MilliunitConverter.Calculate(scheduledTransactionsResponse.Amount);
    public DateTime NextOccurence => scheduledTransactionsResponse.NextOccurence;
    public Guid AccountId => scheduledTransactionsResponse.AccountId;
}