using System.Text.Json.Serialization;

namespace YnabSharp.Responses.Accounts;

public class GetAccountsResponseData
{
    [JsonPropertyName("accounts")]
    public required IEnumerable<AccountResponse> Accounts { get; set; }
}