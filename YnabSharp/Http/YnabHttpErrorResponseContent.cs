using System.Text.Json.Serialization;

namespace YnabSharp.Http;

public class YnabHttpErrorResponseContent
{
    [JsonPropertyName("error")]
    public required YnabHttpError Error { get; set; }
}