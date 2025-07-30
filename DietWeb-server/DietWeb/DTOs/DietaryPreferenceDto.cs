namespace DietWeb.API.DTOs
{
    public class DietaryPreferenceDto
    {
        public string FoodName { get; set; } = "";
        public int UserId { get; set; }
        public string Like { get; set; } = "";
    }
}
