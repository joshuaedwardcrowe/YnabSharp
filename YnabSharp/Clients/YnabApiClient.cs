using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using YnabSharp.Converters;
using YnabSharp.Exceptions;
using YnabSharp.Http;

namespace YnabSharp.Clients;

public abstract class YnabApiClient
{
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true,
        Converters = { 
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
            new IgnoreEmptyStringNullableEnumConverter()
        }
    };

    protected abstract HttpClient GetHttpClient();

    protected async Task<YnabHttpResponseContent<TApiResponse>> Get<TApiResponse>(string path)
        where TApiResponse : class
    {
        var client = GetHttpClient();
        var response = await client.GetAsync(path);
        await EnsureSuccessfulResponse(response);
        return await ReadResponseContent<TApiResponse>(response);
    }

    protected async Task<YnabHttpResponseContent<TApiResponse>> Post<TApiRequest, TApiResponse>(string path, TApiRequest payload)
        where TApiRequest : class where TApiResponse : class
    {
        var client = GetHttpClient();
        var response = await client.PostAsJsonAsync(path, payload, _jsonOptions);
        await EnsureSuccessfulResponse(response);
        return await ReadResponseContent<TApiResponse>(response);
    }

    protected async Task<YnabHttpResponseContent<TApiResponse>> Patch<TApiRequest, TApiResponse>(string path, TApiRequest payload) 
        where TApiRequest : class where TApiResponse : class
    {
        var client = GetHttpClient();
        var response = await client.PatchAsJsonAsync(path, payload, _jsonOptions);
        await EnsureSuccessfulResponse(response);
        return await ReadResponseContent<TApiResponse>(response);
    }

    protected async Task<YnabHttpResponseContent<TApiResponse>> Put<TApiRequest, TApiResponse>(string path, TApiRequest payload)
        where TApiRequest : class where TApiResponse : class
    {
        var client = GetHttpClient();        
        var response = await client.PutAsJsonAsync(path, payload, _jsonOptions);
        await EnsureSuccessfulResponse(response);
        return await ReadResponseContent<TApiResponse>(response);
    }
    
    private async Task EnsureSuccessfulResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadFromJsonAsync<YnabHttpErrorResponseContent>(_jsonOptions);
            if (content is null)
            {
                throw new YnabException(YnabExceptionCode.ApiResponseIsEmpty, $"No error response on {response.RequestMessage?.RequestUri}");
            }
            
            throw new YnabException(
                YnabExceptionCode.ApiResponseIsError,
                $"YNAB API Error {content.Error.Id} ({content.Error.Name}): {content.Error.Detail}");
        }
    }

    private async Task<YnabHttpResponseContent<TApiResponse>> ReadResponseContent<TApiResponse>(HttpResponseMessage response)
        where TApiResponse : class
    {
        var responseContent = await response.Content.ReadFromJsonAsync<YnabHttpResponseContent<TApiResponse>>(_jsonOptions);
        if (responseContent is null)
        {
            throw new YnabException(YnabExceptionCode.ApiResponseIsEmpty, $"No response on {response.RequestMessage?.RequestUri}");
        }
        return responseContent;
    }
}