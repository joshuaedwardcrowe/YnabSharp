using YnabSharpa.Responses.Transactions;

namespace YnabSharpa.Factories;

public interface ITransactionFactory
{
    bool CanWorkWith(TransactionResponse transactionResponse);
    
    Transaction Create(TransactionResponse transactionResponse);
}