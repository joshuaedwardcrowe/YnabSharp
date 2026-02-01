using YnabSharp.Responses.Transactions;

namespace YnabSharp.Factories;

// TODO: Write unit tests.
public class DefaultTransactionFactory : ITransactionFactory
{
    // TODO: Default strategy.
    public bool CanWorkWith(TransactionResponse transactionResponse)
        => true;

    public Transaction Create(TransactionResponse transactionResponse)
        => new(transactionResponse);
}