namespace YnabSharpa.Collections;

public class TransactionsByFlag
{
    public required string Flag { get; set; }
    public required List<Transaction> Transactions { get; set; }
}