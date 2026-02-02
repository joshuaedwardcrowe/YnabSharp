using System.Text.Json.Serialization;

namespace YnabSharp.Requests.Transactions;

public class TransactionRequest
{
    [JsonPropertyName("id")] 
    public string Id { get; set; }

    [JsonPropertyName("memo")]
    public string? Memo { get; set; }
    
    [JsonPropertyName("date")]
    public DateTime Occured { get; set; }
    
    [JsonPropertyName("amount")]
    public int Amount { get; set; }
    
    [JsonPropertyName("payee_id")]
    public Guid? PayeeId { get; set; }
    
    [JsonPropertyName("payee_name")]
    public string? PayeeName { get; set; }
    
    [JsonPropertyName("category_id")]
    public Guid? CategoryId { get; set; }
    
    [JsonPropertyName("category_name")]
    public string? CategoryName { get; set; }
    
    [JsonPropertyName("account_id")]
    public Guid? AccountId { get; set; }
}