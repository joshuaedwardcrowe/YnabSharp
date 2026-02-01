using System.Text.Json.Serialization;

namespace YnabSharp.Responses.Accounts;

public class CreateAccountResponse
{
    [JsonPropertyName("account")]
    public required AccountResponse Account { get; set; }
}

