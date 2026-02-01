using YnabSharp.Responses.Transactions;

namespace YnabSharp.Factories;

public interface ITransactionFactory
{
    bool CanWorkWith(TransactionResponse transactionResponse);
    
    Transaction Create(TransactionResponse transactionResponse);
}