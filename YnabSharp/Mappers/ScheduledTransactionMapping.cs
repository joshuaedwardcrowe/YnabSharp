namespace YnabSharp.Mappers;

public static class ScheduledTransactionMapping
{
    public static MovedScheduledTransaction ToMovedTransaction(this ScheduledTransaction transaction, Guid newAccountId)
        => new(transaction.Id, newAccountId);

    public static IEnumerable<MovedScheduledTransaction> ToMovedTransactions(
        this IEnumerable<ScheduledTransaction> transactions, Guid newAccountId)
        => transactions.Select(transaction => transaction.ToMovedTransaction(newAccountId));
}