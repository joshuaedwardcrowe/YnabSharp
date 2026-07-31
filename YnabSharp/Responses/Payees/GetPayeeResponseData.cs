using System.Text.Json.Serialization;

namespace YnabSharp.Responses.Payees;

public class GetPayeeResponseData
{
    [JsonPropertyName("payee")]
    public required PayeeResponse Payee { get; set; }
}
