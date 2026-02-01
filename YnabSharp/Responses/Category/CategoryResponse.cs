using System.Text.Json.Serialization;

namespace YnabSharpa.Responses.Category;

public class CategoryResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// Money in this category assigned.
    /// </summary>
    public int Assigned => Budgeted;
    
    [JsonPropertyName("budgeted")]
    public int Budgeted { get; set; }

    /// <summary>
    /// Money in this category available to spend.
    /// </summary>
    public int Available => Balance;
    
    [JsonPropertyName("balance")]
    public int Balance { get; set; }
    
    [JsonPropertyName("goal_target")]
    public int GoalTarget { get; set; }
    
    [JsonPropertyName("goal_creation_month")]
    public DateOnly? GoalCreationMonth { get; set; }
    
    [JsonPropertyName("goal_target_month")]
    public DateOnly? GoalTargetMonth { get; set; }
    
    [JsonPropertyName("goal_overall_funded")]
    public int? GoalOverallFunded { get; set; }
    
    [JsonPropertyName("goal_overall_left")]
    public int? GoalOverallLeft { get; set; }
}