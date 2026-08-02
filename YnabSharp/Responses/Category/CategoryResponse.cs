using System.Text.Json.Serialization;

namespace YnabSharp.Responses.Category;

public class CategoryResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("category_group_id")]
    public required Guid CategoryGroupId { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("hidden")]
    public required bool Hidden { get; set; }

    [JsonPropertyName("internal")]
    public required bool Internal { get; set; }

    [JsonPropertyName("deleted")]
    public required bool Deleted { get; set; }

    [JsonPropertyName("activity")]
    public required int Activity { get; set; }

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
    public int? GoalTarget { get; set; }
    
    [JsonPropertyName("goal_creation_month")]
    public DateOnly? GoalCreationMonth { get; set; }
    
    [JsonPropertyName("goal_target_month")]
    public DateOnly? GoalTargetMonth { get; set; }
    
    [JsonPropertyName("goal_overall_funded")]
    public int? GoalOverallFunded { get; set; }
    
    [JsonPropertyName("goal_overall_left")]
    public int? GoalOverallLeft { get; set; }
}