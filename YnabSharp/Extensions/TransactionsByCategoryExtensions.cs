using YnabSharpa.Collections;

namespace YnabSharpa.Extensions;

public static class TransactionsByCategoryExtensions
{
    public static IEnumerable<TransactionsByYearByCategory> GroupByYear(
        this IEnumerable<TransactionsByCategory> transactionGroups)
    {
        foreach (var transactionGroup in transactionGroups)
        {
            var transactionsSubGroups = transactionGroup.Transactions.GroupByYear();
            yield return new TransactionsByYearByCategory(transactionGroup.CategoryName, transactionsSubGroups);
        }
    }
}