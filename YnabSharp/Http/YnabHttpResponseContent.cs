using System.Text.Json.Serialization;

namespace YnabSharp.Http;

public class YnabHttpResponseContent<TResponseData> where TResponseData : class
{
    [JsonPropertyName("data")]
    public required TResponseData Data { get; set; }
}