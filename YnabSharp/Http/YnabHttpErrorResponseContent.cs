using System.Text.Json.Serialization;

namespace YnabSharpa.Http;

public class YnabHttpErrorResponseContent
{
    [JsonPropertyName("error")]
    public required YnabHttpError Error { get; set; }
}