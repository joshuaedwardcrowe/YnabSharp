namespace YnabSharpa.Collections;

public class TransactionsByPayeeName
{
    public required string PayeeName { get; set; }
    public required IEnumerable<Transaction> Transactions { get; set; }
    
    public  IEnumerable<TransactionsByMemoOccurrence> GroupByMemoOccurrence() 
        => Transactions
            .GroupBy(transaction => transaction.Memo)
            .Select(transactionGroup => new TransactionsByMemoOccurrence
            {
                Memo = transactionGroup.Key,
                MemoOccurence = transactionGroup.Count(),
                Transactions = transactionGroup.ToList()
            });
}