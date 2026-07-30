using YnabSharp.Connected;
using YnabSharp.Factories;
using YnabSharp.Http;
using YnabSharp.Responses.Plans;

namespace YnabSharp.Clients;

// TODO: Write unit tests.
public class PlansClient : YnabApiClient
{
    private readonly YnabHttpClientBuilder _httpClientBuilder;
    private readonly IEnumerable<ITransactionFactory> _transactionFactories;

    public PlansClient(
        YnabHttpClientBuilder httpClientBuilder,
        IEnumerable<ITransactionFactory> transactionFactories)
    {
        _httpClientBuilder = httpClientBuilder;
        _transactionFactories = transactionFactories;
    }

    public PlansClient(YnabHttpClientBuilder httpClientBuilder)
    {
        _httpClientBuilder = httpClientBuilder;
        _transactionFactories = new List<ITransactionFactory>();
    }

    public async Task<IEnumerable<ConnectedPlan>> GetPlans()
    {
        var response = await Get<GetPlansResponseData>(string.Empty);
        return ConvertPlanResponsesToWrappers(response.Data.Plans);
    }

    public async Task<ConnectedPlan> GetPlan(Guid planId)
    {
        var response = await Get<GetPlanResponseData>($"{planId}");
        return ConvertPlanResponsesToWrappers([response.Data.Plan]).First();
    }

    public async Task<ConnectedPlan?> GetPlan(string planName)
    {
        var allPlans = await GetPlans();
        return allPlans.FirstOrDefault(p => p.Name == planName);
    }

    private IEnumerable<ConnectedPlan> ConvertPlanResponsesToWrappers(IEnumerable<PlanResponse> planResponses)
    {
        foreach (var planResponse in planResponses)
        {
            var ynabPlanApiPath = $"{YnabApiPath.Plans}/{planResponse.Id}";

            // TODO: Potentially have these in their own methods.
            var accountClient = new AccountClient(_httpClientBuilder, ynabPlanApiPath, _transactionFactories);
            var categoryClient = new CategoryClient(_httpClientBuilder, ynabPlanApiPath);
            var payeeClient = new PayeeClient(_httpClientBuilder, ynabPlanApiPath);
            var transactionClient = new TransactionClient(_httpClientBuilder, ynabPlanApiPath, _transactionFactories);
            var scheduledTransactionClient = new ScheduledTransactionClient(_httpClientBuilder, ynabPlanApiPath);

            yield return new ConnectedPlan(
                accountClient,
                categoryClient,
                payeeClient,
                transactionClient,
                scheduledTransactionClient,
                planResponse);
        }
    }

    protected override HttpClient GetHttpClient() => _httpClientBuilder.Build(
        // is already http://ynab.api/v1/
        null,
        YnabApiPath.Plans);
}