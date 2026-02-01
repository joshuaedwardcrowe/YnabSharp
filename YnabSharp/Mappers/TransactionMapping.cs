namespace YnabSharp.Mappers;

public static class TransactionMapping
{
    public static MovedTransaction ToMovedTransaction(this Transaction transaction, Guid newAccountId)
        => new(transaction.Id, newAccountId);

    public static IEnumerable<MovedTransaction> ToMovedTransactions(
        this IEnumerable<Transaction> transactions, Guid newAccountId)
        => transactions.Select(transaction => transaction.ToMovedTransaction(newAccountId));
}