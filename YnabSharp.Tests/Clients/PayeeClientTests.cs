using System.Text.Json;
using YnabSharp.Clients;
using YnabSharp.Http;
using YnabSharp.Responses.Payees;
using YnabSharp.Tests.TestHelpers;

namespace YnabSharp.Tests.Clients;

[TestFixture]
public class PayeeClientTests
{
    private const string YnabPlanApiPath = "plans/plan-id";

    private static PayeeClient CreateClient(string jsonContent, out TestHttpMessageHandler handler)
    {
        handler = new TestHttpMessageHandler(jsonContent);
        var builder = new YnabHttpClientBuilder(new TestHttpClientFactory(handler)).WithBearerToken("token");
        return new PayeeClient(builder, YnabPlanApiPath);
    }

    private static PayeeResponse CreatePayeeResponse(Guid payeeId, Guid? transferAccountId = null) => new()
    {
        Id = payeeId,
        Name = "Test Payee",
        TransferAccountId = transferAccountId,
        Deleted = false
    };

    private static string SerializePayeesResponse(IEnumerable<PayeeResponse> payeeResponses) =>
        JsonSerializer.Serialize(new YnabHttpResponseContent<GetPayeesResponseData>
        {
            Data = new GetPayeesResponseData { Payees = payeeResponses }
        });

    [Test]
    public async Task GivenPayeesWrapperResponse_WhenGetAll_DeserializesIntoPayees()
    {
        var payeeId = Guid.NewGuid();
        var json = SerializePayeesResponse([CreatePayeeResponse(payeeId)]);
        var client = CreateClient(json, out _);

        var payees = (await client.GetAll()).ToList();

        Assert.That(payees, Has.Count.EqualTo(1));
        Assert.That(payees[0].Id, Is.EqualTo(payeeId));
        Assert.That(payees[0].Name, Is.EqualTo("Test Payee"));
        Assert.That(payees[0].TransferAccountId, Is.Null);
        Assert.That(payees[0].Deleted, Is.False);
    }

    [Test]
    public async Task GivenTransferPayee_WhenGetAll_ExposesTransferAccountId()
    {
        var payeeId = Guid.NewGuid();
        var transferAccountId = Guid.NewGuid();
        var json = SerializePayeesResponse([CreatePayeeResponse(payeeId, transferAccountId)]);
        var client = CreateClient(json, out _);

        var payees = (await client.GetAll()).ToList();

        Assert.That(payees[0].TransferAccountId, Is.EqualTo(transferAccountId));
    }

    [Test]
    public async Task GivenPlanApiPath_WhenGetAll_RequestsPayeesPathUnderPlan()
    {
        var json = SerializePayeesResponse([]);
        var client = CreateClient(json, out var handler);

        await client.GetAll();

        var requestedPath = handler.RequestedUri!.ToString();
        Assert.That(requestedPath, Does.Contain($"{YnabPlanApiPath}/{YnabApiPath.Payees}"));
    }
}
