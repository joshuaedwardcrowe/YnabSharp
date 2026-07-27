using System.Text.Json;
using YnabSharp.Clients;
using YnabSharp.Connected;
using YnabSharp.Http;
using YnabSharp.Responses.Plans;
using YnabSharp.Tests.TestHelpers;

namespace YnabSharp.Tests.Clients;

[TestFixture]
public class PlansClientTests
{
    private static PlansClient CreateClient(string jsonContent, out TestHttpMessageHandler handler)
    {
        handler = new TestHttpMessageHandler(jsonContent);
        var builder = new YnabHttpClientBuilder(new TestHttpClientFactory(handler)).WithBearerToken("token");
        return new PlansClient(builder);
    }

    private static PlanResponse CreatePlanResponse(Guid planId) => new()
    {
        Id = planId,
        Name = "Test Plan",
        FirstMonth = new DateOnly(2024, 1, 1),
        LastMonth = new DateOnly(2024, 6, 1)
    };

    private static string SerializePlanResponse(PlanResponse planResponse) =>
        JsonSerializer.Serialize(new YnabHttpResponseContent<GetPlanResponseData>
        {
            Data = new GetPlanResponseData { Plan = planResponse }
        });

    private static string SerializePlansResponse(IEnumerable<PlanResponse> planResponses) =>
        JsonSerializer.Serialize(new YnabHttpResponseContent<GetPlansResponseData>
        {
            Data = new GetPlansResponseData { Plans = planResponses }
        });

    [Test]
    public async Task GivenPlanId_WhenGetPlan_RequestsPlansPathWithNoDuplicatedSegment()
    {
        var planId = Guid.NewGuid();
        var json = SerializePlanResponse(CreatePlanResponse(planId));
        var client = CreateClient(json, out var handler);

        await client.GetPlan(planId);

        var requestedPath = handler.RequestedUri!.ToString();
        Assert.That(requestedPath, Does.Contain($"plans/{planId}"));
        Assert.That(requestedPath, Does.Not.Contain("plans/plans"));
    }

    [Test]
    public async Task GivenPlansWrapperResponse_WhenGetPlans_DeserializesIntoConnectedPlans()
    {
        var planId = Guid.NewGuid();
        var json = SerializePlansResponse([CreatePlanResponse(planId)]);
        var client = CreateClient(json, out _);

        var plans = (await client.GetPlans()).ToList();

        Assert.That(plans, Has.Count.EqualTo(1));
        Assert.That(plans[0], Is.InstanceOf<ConnectedPlan>());
        Assert.That(plans[0].Id, Is.EqualTo(planId));
        Assert.That(plans[0].Name, Is.EqualTo("Test Plan"));
    }

    [Test]
    public async Task GivenPlanWrapperResponse_WhenGetPlanByGuid_DeserializesIntoConnectedPlan()
    {
        var planId = Guid.NewGuid();
        var json = SerializePlanResponse(CreatePlanResponse(planId));
        var client = CreateClient(json, out _);

        var plan = await client.GetPlan(planId);

        Assert.That(plan, Is.InstanceOf<ConnectedPlan>());
        Assert.That(plan.Id, Is.EqualTo(planId));
        Assert.That(plan.Name, Is.EqualTo("Test Plan"));
    }
}
