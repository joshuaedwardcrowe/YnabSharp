using YnabSharp.Factories;
using YnabSharp.Http;
using YnabSharp.Mappers;
using YnabSharp.Requests.Transactions;
using YnabSharp.Responses.Transactions;

namespace YnabSharp.Clients;

// TODO: Write unit tests.
public class TransactionClient : YnabApiClient
{
    private readonly YnabHttpClientBuilder _httpClientBuilder;
    private readonly string _ynabBudgetApiPath;
    private readonly IEnumerable<ITransactionFactory> _transactionFactories;

    public TransactionClient(
        YnabHttpClientBuilder httpClientBuilder,
        string ynabBudgetApiPath,
        IEnumerable<ITransactionFactory> transactionFactories)
    {
        _httpClientBuilder = httpClientBuilder;
        _ynabBudgetApiPath = ynabBudgetApiPath;
        _transactionFactories = transactionFactories;
    }

    public async Task<IEnumerable<Transaction>> GetAll()
    {
        var response = await Get<GetTransactionsResponse>(string.Empty);
        return response.Data.Transactions.Select(CreateTransaction);
    }

    public async Task<Transaction> Get(string transactionId)
    {
        var response = await Get<GetTransactionResponse>($"{transactionId}");
        return CreateTransaction(response.Data.Transaction);
    }

    public async Task<IEnumerable<Transaction>> Move(IEnumerable<MovedTransaction> movedTransactions)
    {
        var request = new UpdateTransactionRequest(movedTransactions.ToTransactionRequests());
        var response = await Patch<UpdateTransactionRequest, GetTransactionsResponse>(string.Empty, request);
        return response.Data.Transactions.Select(CreateTransaction);
    }

    private Transaction CreateTransaction(TransactionResponse transactionResponse)
        => _transactionFactories
            .First(s => s.CanWorkWith(transactionResponse))
            .Create(transactionResponse);
    
    protected override HttpClient GetHttpClient() => _httpClientBuilder.Build(_ynabBudgetApiPath,  YnabApiPath.Transactions);
}

