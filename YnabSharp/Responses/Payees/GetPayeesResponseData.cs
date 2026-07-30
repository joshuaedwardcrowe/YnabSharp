using System.Text.Json.Serialization;

namespace YnabSharp.Responses.Payees;

public class GetPayeesResponseData
{
    [JsonPropertyName("payees")]
    public required IEnumerable<PayeeResponse> Payees { get; set; }
}
