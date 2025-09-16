using DietWeb.Core.Models;

namespace DietWeb.API.DTOs
{
    public class UpdateUserDto
    {
        public float? StartWeight { get; set; }
        public float? CurrentWeight { get; set; }
        public float? GoalWeight { get; set; }
        public int? Height { get; set; }
        public string? ChatPersonality { get; set; }
        public string? ProgramLevel { get; set; }
        public List<DietaryPreferenceDto>? DietaryPreferences { get; set; }
        public List<FavoriteRecipeDto>? FavoriteRecipes { get; set; }

    }

}
