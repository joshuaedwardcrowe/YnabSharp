using YnabSharp.Connected;
using YnabSharp.Factories;
using YnabSharp.Http;
using YnabSharp.Mappers;
using YnabSharp.Responses.Accounts;

namespace YnabSharp.Clients;

// TODO: Write unit tests.
public class AccountClient : YnabApiClient
{
    private readonly YnabHttpClientBuilder _httpClientBuilder;
    private readonly string _ynabPlanApiPath;
    private readonly IEnumerable<ITransactionFactory> _transactionFactories;

    public AccountClient(
        YnabHttpClientBuilder httpClientBuilder,
        string ynabPlanApiPath,
        IEnumerable<ITransactionFactory> transactionFactories)
    {
        _httpClientBuilder = httpClientBuilder;
        _ynabPlanApiPath = ynabPlanApiPath;
        _transactionFactories = transactionFactories;
    }

    public async Task<IEnumerable<Account>> GetAll()
    {
        var response = await Get<GetAccountsResponseData>(string.Empty);
        return response.Data.Accounts.Select(a => new Account(a));
    }
    
    public async Task<ConnectedAccount> Get(Guid accountId)
    {
        var response = await Get<GetAccountResponseData>($"{accountId}");
        return ConvertAccountResponseToConnectedAccount(response.Data.Account);
    }
    
    public async Task<ConnectedAccount> Create(NewAccount newAccount)
    {
        var request = new CreateAccountRequest(newAccount.ToAccountRequest());
        var response = await Post<CreateAccountRequest, CreateAccountResponse>(string.Empty, request);
        return ConvertAccountResponseToConnectedAccount(response.Data.Account);
    }

    public async Task<ConnectedAccount> Create(Account account)
    {
        var request = new CreateAccountRequest(account.ToAccountRequest());
        var response = await Post<CreateAccountRequest, CreateAccountResponse>(string.Empty, request);
        return ConvertAccountResponseToConnectedAccount(response.Data.Account);
    }
    
    private ConnectedAccount ConvertAccountResponseToConnectedAccount(AccountResponse accountResponse)
    {
        var transactionClient = new TransactionClient(_httpClientBuilder, _ynabPlanApiPath, _transactionFactories);
        var scheduledTransactionClient = new ScheduledTransactionClient(_httpClientBuilder, _ynabPlanApiPath);
        return new ConnectedAccount(transactionClient, scheduledTransactionClient, accountResponse);
    }

    protected override HttpClient GetHttpClient() => _httpClientBuilder.Build(
        // e.g. plans/{planId}
        _ynabPlanApiPath,
        YnabApiPath.Accounts);
}