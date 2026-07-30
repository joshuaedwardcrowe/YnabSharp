using YnabSharp.Http;
using YnabSharp.Responses.Payees;

namespace YnabSharp.Clients;

public class PayeeClient(YnabHttpClientBuilder builder, string ynabPlanApiPath) : YnabApiClient
{
    public async Task<IEnumerable<Payee>> GetAll()
    {
        var response = await Get<GetPayeesResponseData>(string.Empty);
        return response.Data.Payees.Select(payee => new Payee(payee));
    }

    protected override HttpClient GetHttpClient() => builder.Build(ynabPlanApiPath, YnabApiPath.Payees);
}
