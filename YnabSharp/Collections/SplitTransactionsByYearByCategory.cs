namespace YnabSharpa.Collections;

public record SplitTransactionsByYearByCategory(Guid CategoryId, IEnumerable<SplitTransactionsByYear> TransactionsByYear);