namespace YnabSharpa.Collections;

public record TransactionsByCategory(string CategoryName, IEnumerable<Transaction> Transactions);