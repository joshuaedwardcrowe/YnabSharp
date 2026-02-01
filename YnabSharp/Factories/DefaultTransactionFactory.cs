using YnabSharpa.Responses.Transactions;

namespace YnabSharpa.Factories;

// TODO: Write unit tests.
public class DefaultTransactionFactory : ITransactionFactory
{
    // TODO: Default strategy.
    public bool CanWorkWith(TransactionResponse transactionResponse)
        => true;

    public Transaction Create(TransactionResponse transactionResponse)
        => new(transactionResponse);
}