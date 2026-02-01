using YnabSharp.Collections;

namespace YnabSharp.Extensions;

public static class TransactionsByPayeeNameExtensions
{
    public static IEnumerable<TransactionsByMemoOccurrenceByPayeeName> GroupByPayeeMemoOccurence(
        this IEnumerable<TransactionsByPayeeName> transactionsByPayeeNames)
    {
        foreach (var transactionsByPayeeName in transactionsByPayeeNames)
        {
            var memoOccurrenceGroups = transactionsByPayeeName
                .GroupByMemoOccurrence()
                .OrderByDescending(memoOccurrenceGroup => memoOccurrenceGroup.MemoOccurence);

            yield return new TransactionsByMemoOccurrenceByPayeeName(
                transactionsByPayeeName.PayeeName,
                memoOccurrenceGroups);
        }
    }
}