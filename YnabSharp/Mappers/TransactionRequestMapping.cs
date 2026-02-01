using YnabSharp.Requests.Transactions;

namespace YnabSharp.Mappers;

public static class TransactionRequestMapping
{
    public static TransactionRequest ToTransactionRequest(
        this MovedTransaction movedScheduledTransaction)
            => new(movedScheduledTransaction.Id, movedScheduledTransaction.AccountId);
    
    public static IEnumerable<TransactionRequest> ToTransactionRequests(
        this IEnumerable<MovedTransaction> movedScheduledTransactions)
            => movedScheduledTransactions
                .Select(movedScheduledTransaction => movedScheduledTransaction.ToTransactionRequest());
}