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

    [Test]
    public async Task GivenPayeesWrapperResponse_WhenGetAll_ThenDeserializesIntoPayees()
    {
        var payeeId = Guid.NewGuid();
        var json = SerializePayeesResponse([CreatePayeeResponse(payeeId)]);
        var client = CreateClient(json, out _);

        var response = await client.GetAll();
        var payees = response.ToList();
        var firstPayee = payees.FirstOrDefault();

        Assert.That(payees, Has.Count.EqualTo(1));
        Assert.That(firstPayee!.Id, Is.EqualTo(payeeId));
        Assert.That(firstPayee.Name, Is.EqualTo("Test Payee"));
        Assert.That(firstPayee.TransferAccountId, Is.Null);
        Assert.That(firstPayee.Deleted, Is.False);
    }

    [Test]
    public async Task GivenTransferPayee_WhenGetAll_ThenExposesTransferAccountId()
    {
        var payeeId = Guid.NewGuid();
        var transferAccountId = Guid.NewGuid();
        var json = SerializePayeesResponse([CreatePayeeResponse(payeeId, transferAccountId)]);
        var client = CreateClient(json, out _);

        var response = await client.GetAll();
        var firstPayee = response.FirstOrDefault();

        Assert.That(firstPayee!.TransferAccountId, Is.EqualTo(transferAccountId));
    }

    [Test]
    public async Task GivenPlanApiPath_WhenGetAll_ThenRequestsPayeesPathUnderPlan()
    {
        var json = SerializePayeesResponse([]);
        var client = CreateClient(json, out var handler);

        await client.GetAll();

        var requestedPath = handler.RequestedUri!.ToString();
        Assert.That(requestedPath, Does.Contain($"{YnabPlanApiPath}/{YnabApiPath.Payees}"));
    }

    [Test]
    public async Task GivenPayeeWrapperResponse_WhenGet_ThenDeserializesIntoPayee()
    {
        var payeeId = Guid.NewGuid();
        var json = SerializePayeeResponse(CreatePayeeResponse(payeeId));
        var client = CreateClient(json, out _);

        var payee = await client.Get(payeeId);

        Assert.That(payee.Id, Is.EqualTo(payeeId));
        Assert.That(payee.Name, Is.EqualTo("Test Payee"));
        Assert.That(payee.TransferAccountId, Is.Null);
        Assert.That(payee.Deleted, Is.False);
    }

    [Test]
    public async Task GivenTransferPayee_WhenGet_ThenExposesTransferAccountId()
    {
        var payeeId = Guid.NewGuid();
        var transferAccountId = Guid.NewGuid();
        var json = SerializePayeeResponse(CreatePayeeResponse(payeeId, transferAccountId));
        var client = CreateClient(json, out _);

        var payee = await client.Get(payeeId);

        Assert.That(payee.TransferAccountId, Is.EqualTo(transferAccountId));
    }

    [Test]
    public async Task GivenPayeeId_WhenGet_ThenRequestsPayeeIdPathUnderPlan()
    {
        var payeeId = Guid.NewGuid();
        var json = SerializePayeeResponse(CreatePayeeResponse(payeeId));
        var client = CreateClient(json, out var handler);

        await client.Get(payeeId);

        var requestedPath = handler.RequestedUri!.ToString();
        Assert.That(requestedPath, Does.Contain($"{YnabPlanApiPath}/{YnabApiPath.Payees}/{payeeId}"));
    }

    private static PayeeClient CreateClient(string jsonContent, out TestHttpMessageHandler handler)
    {
        handler = new TestHttpMessageHandler(jsonContent);
        var httpClientFactory = new TestHttpClientFactory(handler);
        var builder = new YnabHttpClientBuilder(httpClientFactory)
            .WithBearerToken("token");
        return new PayeeClient(builder, YnabPlanApiPath);
    }

    private static PayeeResponse CreatePayeeResponse(Guid payeeId, Guid? transferAccountId = null)
        => new()
        {
            Id = payeeId,
            Name = "Test Payee",
            TransferAccountId = transferAccountId,
            Deleted = false
        };

    private static string SerializePayeesResponse(IEnumerable<PayeeResponse> payeeResponses)
        => JsonSerializer.Serialize(new YnabHttpResponseContent<GetPayeesResponseData>
        {
            Data = new GetPayeesResponseData { Payees = payeeResponses }
        });

    private static string SerializePayeeResponse(PayeeResponse payeeResponse)
        => JsonSerializer.Serialize(new YnabHttpResponseContent<GetPayeeResponseData>
        {
            Data = new GetPayeeResponseData { Payee = payeeResponse }
        });
}
