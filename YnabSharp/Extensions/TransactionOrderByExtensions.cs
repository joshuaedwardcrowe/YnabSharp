namespace YnabSharpa.Extensions;

public static class TransactionOrderByExtensions
{
    public static IEnumerable<Transaction> OrderByYearAscending(
        this IEnumerable<Transaction> transactions)
            => transactions.OrderBy(transaction => transaction.Occured.Year);
}