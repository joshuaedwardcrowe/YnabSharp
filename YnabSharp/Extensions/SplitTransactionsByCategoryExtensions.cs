using YnabSharp.Collections;

namespace YnabSharp.Extensions;

public static class SplitTransactionsByCategoryExtensions
{
    public static IEnumerable<SplitTransactionsByYearByCategory> GroupByYear(
        this IEnumerable<SplitTransactionsByCategory> transactionGroups) 
        => transactionGroups
            .Select(group =>
            {
                var transactionsByYear = group.SplitTransactions.GroupByYear();
                return new SplitTransactionsByYearByCategory(group.CategoryId, transactionsByYear);
            });
}