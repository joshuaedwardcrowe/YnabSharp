using YnabSharpa.Responses.Transactions;

namespace YnabSharpa;

public class Transaction(TransactionResponse transactionResponse) : SplitTransactions(transactionResponse)
{
    public string? FlagName => transactionResponse.FlagName;
    public FlagColor? FlagColour => transactionResponse.FlagColor;
    public IEnumerable<SplitTransactions> SplitTransactions 
        => transactionResponse
            .SplitTransactions
            .Select(splitTransactionResponse => new SplitTransactions(splitTransactionResponse with
            {
                // Splits do not have occured set.
                Occured = transactionResponse.Occured
            }));
}