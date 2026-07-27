using System.Net;
using System.Text;

namespace YnabSharp.Tests.TestHelpers;

public class TestHttpMessageHandler(string jsonContent) : HttpMessageHandler
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