using YnabSharp.Requests.Transactions;
using YnabSharp.Sanitisers;

namespace YnabSharp.Mappers;

public static class TransactionRequestMapping
{
    public static TransactionRequest ToTransactionRequest(
        this Transaction transaction)
            => new()
            {
                Id = transaction.Id,
                AccountId = transaction.AccountId,
                Amount = MilliunitConverter.PoundsToMilliunit(transaction.Amount),
                CategoryId = transaction.CategoryId,
                CategoryName = transaction.CategoryName,
                Occured = transaction.Occured,
                PayeeId = transaction.PayeeId,
                PayeeName = transaction.PayeeName,
                Memo = transaction.Memo
            };

    public static TransactionRequest ToTransactionRequest(
        this MovedTransaction movedScheduledTransaction)
            => new()
            {
                Id = movedScheduledTransaction.Id,
                AccountId = movedScheduledTransaction.AccountId,
            };
    
    public static IEnumerable<TransactionRequest> ToTransactionRequests(
        this IEnumerable<MovedTransaction> movedScheduledTransactions)
            => movedScheduledTransactions
                .Select(movedScheduledTransaction => movedScheduledTransaction.ToTransactionRequest());
}