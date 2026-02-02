using YnabSharp.Connected;
using YnabSharp.Factories;
using YnabSharp.Http;
using YnabSharp.Responses.Budgets;

namespace YnabSharp.Clients;

// TODO: Write unit tests.
public class BudgetsClient : YnabApiClient
{
    private readonly YnabHttpClientBuilder _httpClientBuilder;
    private readonly IEnumerable<ITransactionFactory> _transactionFactories;
    
    public BudgetsClient(
        YnabHttpClientBuilder httpClientBuilder, 
        IEnumerable<ITransactionFactory> transactionFactories)
    {
        _httpClientBuilder = httpClientBuilder;
        _transactionFactories = transactionFactories;
    }
    
    public BudgetsClient(YnabHttpClientBuilder httpClientBuilder)
    {
        _httpClientBuilder = httpClientBuilder;
        _transactionFactories = new List<ITransactionFactory>();
    }

    public async Task<IEnumerable<ConnectedBudget>> GetBudgets()
    {
        var response = await Get<GetBudgetsResponseData>(string.Empty);
        return ConvertBudgetResponsesToWrappers(response.Data.Budgets);
    }

    public async Task<ConnectedBudget> GetBudget(Guid budgetId)
    {
        var response = await Get<GetBudgetResponseData>($"budgets/{budgetId}");
        return ConvertBudgetResponsesToWrappers([response.Data.Budget]).First();
    }

    public async Task<ConnectedBudget?> GetBudget(string budgetName)
    {
        var allBudgets = await GetBudgets();
        return allBudgets.FirstOrDefault(b => b.Name == budgetName);
    }

    private IEnumerable<ConnectedBudget> ConvertBudgetResponsesToWrappers(IEnumerable<BudgetResponse> budgetResponses)
    {
        foreach (var budgetResponse in budgetResponses)
        {
            var ynabBudgetApiPath = $"{YnabApiPath.Budgets}/{budgetResponse.Id}";
            
            // TODO: Potentially have these in their own methods.
            var accountClient = new AccountClient(_httpClientBuilder, ynabBudgetApiPath, _transactionFactories);
            var categoryClient = new CategoryClient(_httpClientBuilder, ynabBudgetApiPath);
            var transactionClient = new TransactionClient(_httpClientBuilder, ynabBudgetApiPath, _transactionFactories);
            var scheduledTransactionClient = new ScheduledTransactionClient(_httpClientBuilder, ynabBudgetApiPath);

            yield return new ConnectedBudget(
                accountClient,
                categoryClient,
                transactionClient,
                scheduledTransactionClient,
                budgetResponse);
        }
    }

    protected override HttpClient GetHttpClient() => _httpClientBuilder.Build(
        // is already http://ynab.api/v1/
        null,
        YnabApiPath.Budgets);
}