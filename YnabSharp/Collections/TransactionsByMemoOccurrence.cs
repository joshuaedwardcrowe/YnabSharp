namespace YnabSharpa.Collections;

public class TransactionsByMemoOccurrence
{
    public string? Memo { get; set; }
    public int MemoOccurence { get; set; }
    public IEnumerable<Transaction> Transactions { get; set; } = new List<Transaction>();
}