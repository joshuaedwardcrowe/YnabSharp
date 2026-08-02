using YnabSharp.Responses.Transactions;

namespace YnabSharp;

public class Transaction(TransactionResponse transactionResponse) : SplitTransactions(transactionResponse)
{
    public string? FlagName => transactionResponse.FlagName;
    public FlagColor? FlagColour => transactionResponse.FlagColor;
    public bool Approved => transactionResponse.Approved;
    public TransactionClearedStatus Cleared => transactionResponse.Cleared;
    public bool Deleted => transactionResponse.Deleted;
    public string AccountName => transactionResponse.AccountName;
    public IEnumerable<SplitTransactions> SplitTransactions 
        => transactionResponse
            .SplitTransactions
            .Select(splitTransactionResponse => new SplitTransactions(splitTransactionResponse with
            {
                // Splits do not have occured set.
                Occured = transactionResponse.Occured
            }));
}