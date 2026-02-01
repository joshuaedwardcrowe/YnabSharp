namespace YnabSharpa;

public static class SplitTransactionExtensions
{
    public static bool AllFullyFormed(this IEnumerable<SplitTransactions> splitTransactions)
        => splitTransactions.All(splitTransaction => splitTransaction.IsFullyFormed);
}