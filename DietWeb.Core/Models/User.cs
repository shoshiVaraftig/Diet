using DietWeb.Core.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string HashedPassword { get; set; }
    public string? Email { get; set; }
    public string ? Phone { get; set; }
    public int? Gender  { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? ProgramLevel { get; set; } 
    public int Height { get; set; }
    public float currentWeight { get; set; }
    public float? StartWeight { get; set; }
    public float? GoalWeight { get; set; }
    public DateTime? GoalDate { get; set; }
    public DateTime? StartDate { get; set; }
    public WeightTracing? WeightTracing { get; set; }
    public List<DietaryPreference>? DietaryPreferences { get; set; } = new();
    public string? ChatPersonality { get; set; }
    public List<FavoriteRecipe>? FavoriteRecipes { get; set; } = new();

}