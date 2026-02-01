using System.Text.Json.Serialization;

namespace YnabSharpa.Responses.Accounts;

public class GetAccountsResponseData
{
    [JsonPropertyName("accounts")]
    public required IEnumerable<AccountResponse> Accounts { get; set; }
}