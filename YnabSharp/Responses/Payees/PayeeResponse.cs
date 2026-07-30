using System.Text.Json.Serialization;

namespace YnabSharp.Responses.Payees;

public record PayeeResponse
{
    [JsonPropertyName("id")]
    public required Guid Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("transfer_account_id")]
    public Guid? TransferAccountId { get; set; }

    [JsonPropertyName("deleted")]
    public required bool Deleted { get; set; }
}
