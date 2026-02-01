using System.Text.Json.Serialization;

namespace YnabSharp.Responses.Transactions;

public record SplitTransactionResponse
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
    
    [JsonPropertyName("memo")]
    public string? Memo { get; set; }
    
    [JsonPropertyName("date")]
    public DateTime Occured { get; set; }
    
    [JsonPropertyName("amount")]
    public int Amount { get; set; }
    
    [JsonPropertyName("payee_id")]
    public Guid? PayeeId { get; set; }
    
    [JsonPropertyName("payee_name")]
    public required string PayeeName { get; set; }
    
    [JsonPropertyName("category_id")]
    public Guid? CategoryId { get; set; }
    
    [JsonPropertyName("category_name")]
    public required string CategoryName { get; set; }
    
    [JsonPropertyName("transfer_transaction_id")]
    public string? TransferTransactionId { get; set; }
    
    [JsonPropertyName("account_id")]
    public Guid? AccountId { get; set; }
}