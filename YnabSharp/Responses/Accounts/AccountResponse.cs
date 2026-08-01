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

    [JsonPropertyName(AccountFieldNames.Balance)]
    public required int Balance { get; set; }

    [JsonPropertyName(AccountFieldNames.UnclearedBalance)]
    public required int UnclearedBalance { get; set; }

    [JsonPropertyName(AccountFieldNames.TransferPayeeId)]
    public required Guid? TransferPayeeId { get; set; }

    [JsonPropertyName(AccountFieldNames.Deleted)]
    public required bool Deleted { get; set; }
}