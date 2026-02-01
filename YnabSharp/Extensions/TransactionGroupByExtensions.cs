using YnabSharp.Collections;
using YnabSharp.Sanitisers;

namespace YnabSharp.Extensions;

public static class TransactionGroupByExtensions
{
    public static IEnumerable<TransactionsByYear> GroupByYear(this IEnumerable<Transaction> transactions)
    {
        var groups = transactions.GroupBy(transaction =>
            IdentifierSanitiser.SanitiseForYear(transaction.Occured));

        foreach (var group in groups)
        {
            yield return new TransactionsByYear(group.Key, group.ToList());
        }
    }
    
    public static IEnumerable<TransactionsByMonth> GroupByMonth(
        this IEnumerable<Transaction> transactions)
    {
        var groups = transactions.GroupBy(transaction =>
            IdentifierSanitiser.SanitiseForMonth(transaction.Occured));

        foreach (var group in groups)
        {
            yield return new TransactionsByMonth
            {
                Month = group.Key,
                Transactions = group
            };
        }
    }
    
    public static IEnumerable<TransactionsByPayeeName> GroupByPayeeName(
        this IEnumerable<Transaction> transactions, string? payeeName = null)
    {
        var groups = transactions
            .GroupBy(transaction => transaction.PayeeName)
            .OrderByDescending(group => group.Count());

        foreach (var group in groups)
        {
            yield return new TransactionsByPayeeName
            {
                PayeeName = group.Key,
                Transactions = group.ToList()
            };
        }
    }
    
    public static IEnumerable<TransactionsByCategory> GroupByCategory(this IEnumerable<Transaction> transactions)
        => transactions
            .GroupBy(transaction => transaction.CategoryName)
            .Select(group => new TransactionsByCategory(group.Key, group.ToList()));
}