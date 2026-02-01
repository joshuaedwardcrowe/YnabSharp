using YnabSharp.Collections;
using YnabSharp.Sanitisers;

namespace YnabSharp.Extensions;

public static class SplitTransactionGroupByExtensions
{
    public static IEnumerable<SplitTransactionsByCategory> GroupByCategory(
        this IEnumerable<SplitTransactions> splitTransactions)
        => splitTransactions
            .Where(transaction => transaction.CategoryId.HasValue)
            .GroupBy(transaction => transaction.CategoryId)
            .Select(group => new SplitTransactionsByCategory(group.Key!.Value, group.ToList()));
    
    public static IEnumerable<SplitTransactionsByYear> GroupByYear(this IEnumerable<SplitTransactions> transactions)
    {
        var groups = transactions.GroupBy(transaction =>
            IdentifierSanitiser.SanitiseForYear(transaction.Occured));

        foreach (var group in groups)
        {
            yield return new SplitTransactionsByYear(group.Key, group.ToList());
        }
    }
}