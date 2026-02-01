namespace YnabSharp.Collections;

public record TransactionsByYearByCategory(string CategoryName, IEnumerable<TransactionsByYear> TransactionsByYear);