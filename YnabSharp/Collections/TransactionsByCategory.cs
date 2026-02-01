namespace YnabSharp.Collections;

public record TransactionsByCategory(string CategoryName, IEnumerable<Transaction> Transactions);