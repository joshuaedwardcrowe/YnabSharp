using YnabSharp.Http;
using YnabSharp.Responses.Payees;

namespace YnabSharp.Clients;

public class PayeeClient(YnabHttpClientBuilder httpClientBuilder, string ynabPlanApiPath) : YnabApiClient
{
    /// <summary>Returns all payees for the plan.</summary>
    public async Task<IEnumerable<Payee>> GetAll()
    {
        var response = await Get<GetPayeesResponseData>(string.Empty);
        return response.Data.Payees.Select(payee => new Payee(payee));
    }

    protected override HttpClient GetHttpClient() => httpClientBuilder.Build(
        // e.g. plans/{planId}
        ynabPlanApiPath,
        YnabApiPath.Payees);
}
