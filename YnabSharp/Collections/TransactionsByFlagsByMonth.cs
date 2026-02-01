namespace YnabSharp.Collections;

public class TransactionsByFlagsByMonth
{
    public required string Month { get; set; }
    public required List<TransactionsByFlag> TransactionsByFlags { get; set; }
}