namespace YnabSharp.Collections;

public record SplitTransactionsByYear(int Year, IEnumerable<SplitTransactions> SplitTransactions)
{
    public decimal TotalAmount => SplitTransactions.Sum(st => st.Amount);
}