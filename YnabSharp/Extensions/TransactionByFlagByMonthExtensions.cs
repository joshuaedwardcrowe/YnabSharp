using YnabSharpa.Collections;

namespace YnabSharpa.Extensions;

public static class TransactionByFlagByMonthExtensions
{
    public static IEnumerable<TransactionsByFlagsByMonth> GroupByFlags(
        this IEnumerable<TransactionsByMonth> transactionsForMonths)
    {
        foreach (var transactionsForMonth in transactionsForMonths)
        {
            var transactionsForFlags = transactionsForMonth.GroupByFlags();

            yield return new TransactionsByFlagsByMonth
            {
                Month= transactionsForMonth.Month,
                TransactionsByFlags = transactionsForFlags.ToList()
            };
        }
    }
}