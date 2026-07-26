using System.Net;
using System.Text;
using YnabSharp.Clients;
using YnabSharp.Connected;
using YnabSharp.Http;

namespace YnabSharp.Tests.Clients;

[TestFixture]
public class PlansClientTests
{
    private class StubHttpMessageHandler(string jsonContent) : HttpMessageHandler
    {
        public Uri? RequestedUri { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            RequestedUri = request.RequestUri;

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };
            return Task.FromResult(response);
        }
    }

    private class StubHttpClientFactory(HttpMessageHandler handler) : IHttpClientFactory
    {
        public HttpClient CreateClient(string name) => new(handler);
    }

    private static PlansClient CreateClient(string jsonContent, out StubHttpMessageHandler handler)
    {
        handler = new StubHttpMessageHandler(jsonContent);
        var builder = new YnabHttpClientBuilder(new StubHttpClientFactory(handler)).WithBearerToken("token");
        return new PlansClient(builder);
    }

    [Test]
    public async Task GivenPlanId_WhenGetPlan_RequestsPlansPathWithNoDuplicatedSegment()
    {
        var planId = Guid.NewGuid();
        var json = $$"""
        {
            "data": {
                "plan": {
                    "id": "{{planId}}",
                    "name": "Test Plan",
                    "first_month": "2024-01-01",
                    "last_month": "2024-06-01"
                }
            }
        }
        """;
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
        var json = $$"""
        {
            "data": {
                "plans": [
                    {
                        "id": "{{planId}}",
                        "name": "Test Plan",
                        "first_month": "2024-01-01",
                        "last_month": "2024-06-01"
                    }
                ]
            }
        }
        """;
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
        var json = $$"""
        {
            "data": {
                "plan": {
                    "id": "{{planId}}",
                    "name": "Test Plan",
                    "first_month": "2024-01-01",
                    "last_month": "2024-06-01"
                }
            }
        }
        """;
        var client = CreateClient(json, out _);

        var plan = await client.GetPlan(planId);

        Assert.That(plan, Is.InstanceOf<ConnectedPlan>());
        Assert.That(plan.Id, Is.EqualTo(planId));
        Assert.That(plan.Name, Is.EqualTo("Test Plan"));
    }
}
