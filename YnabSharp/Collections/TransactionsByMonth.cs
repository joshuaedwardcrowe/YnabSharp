using YnabSharpa.Sanitisers;

namespace YnabSharpa.Collections;

public class TransactionsByMonth
{
    public required string Month { get; set; }
    public required IEnumerable<Transaction> Transactions { get; set; }
    
    public IEnumerable<TransactionsByFlag> GroupByFlags()
        => Transactions
            .Where(t => t.FlagName != null)
            .GroupBy(t => IdentifierSanitiser.SanitiseForFlag(t.FlagName, t.FlagColour))
            .Select(transactionGroup => new TransactionsByFlag
            {
                Flag = transactionGroup.Key,
                Transactions = transactionGroup.ToList()
            });
}