namespace YnabSharp.Collections;

public record SplitTransactionsByCategory(Guid CategoryId, IEnumerable<SplitTransactions> SplitTransactions);