using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietWeb.Core.Models
{
    public class FavoriteRecipe
    {
        public int Id { get; set; } // מזהה ייחודי של המתכון המועדף

        // מפתח זר לקשר למשתמש
        public int UserId { get; set; }
        //public User User { get; set; }

        // מפתח זר לקשר למשתמש
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
