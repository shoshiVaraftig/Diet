// DietWeb.Core/Models/Recipe.cs (או מיקום דומה)

using System.Collections.Generic; // וודאי שזה קיים בשביל List<string>

namespace DietWeb.Core.Models // או ה-namespace הנכון של המודלים שלך
{

    public class Recipe
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string>? Ingredients { get; set; }
        public List<string>? Instructions { get; set; }
        public int? ReadyInMinutes { get; set; }
        public int? Servings { get; set; }
        public bool? Vegetarian { get; set; }
        public bool? Vegan { get; set; }
        public bool? GlutenFree { get; set; }
        public bool? DairyFree { get; set; }
        public string Image { get; set; }
        public int? Likes { get; set; }
    }

}