namespace YnabSharpa.Collections;

public record TransactionsByYearByCategory(string CategoryName, IEnumerable<TransactionsByYear> TransactionsByYear);