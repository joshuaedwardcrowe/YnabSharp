using System.Text.Json.Serialization;

namespace YnabSharp.Responses.Accounts;

public record AccountResponse
{
    [JsonPropertyName(AccountFieldNames.Id)]
    public required Guid Id { get; set; }

    [JsonPropertyName(AccountFieldNames.Name)]
    public required string Name { get; set; }

    [JsonPropertyName(AccountFieldNames.Type)]
    public required AccountType Type { get; set; }

    [JsonPropertyName(AccountFieldNames.ClearedBalance)]
    public required int ClearedBalance { get; set; }

    [JsonPropertyName(AccountFieldNames.Closed)]
    public required bool Closed { get; set; }

    [JsonPropertyName(AccountFieldNames.OnBudget)]
    public required bool OnBudget { get; set; }
}