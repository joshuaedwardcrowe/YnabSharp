using System.Text.Json.Serialization;

namespace YnabSharpa.Responses.Accounts;

public class AccountRequest
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("type")]
    public required AccountType Type { get; set; }

    [JsonPropertyName("balance")]
    public decimal Balance { get; set; }
}