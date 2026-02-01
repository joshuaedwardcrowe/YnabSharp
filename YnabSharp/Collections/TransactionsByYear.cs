namespace YnabSharp.Collections;

public record TransactionsByYear(int Year, IEnumerable<Transaction> Transactions);