using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace DietWeb.Core.Models
{
    public class Recipe
    {

    
        [JsonPropertyName("Title")]
        public string Title { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("Ingredients")]
        public string[] Ingredients { get; set; }

        [JsonPropertyName("Instructions")]
        public string[] Instructions { get; set; }

        [JsonPropertyName("ReadyInMinutes")]
        public int? ReadyInMinutes { get; set; }

        [JsonPropertyName("Servings")]
        public int? Servings { get; set; }

        [JsonPropertyName("Vegetarian")]
        public bool Vegetarian { get; set; }

        [JsonPropertyName("Vegan")]
        public bool Vegan { get; set; }

        [JsonPropertyName("GlutenFree")]
        public bool GlutenFree { get; set; }

        [JsonPropertyName("DairyFree")]
        public bool DairyFree { get; set; }

        [JsonPropertyName("Image")]
        public string Image { get; set; }

        [JsonPropertyName("Likes")]
        public int Likes { get; set; }

        // זה שדה למסד הנתונים בלבד, נתעלם ממנו ב-JSON
        [JsonIgnore]
        public int Id { get; set; }
    }
}


