using DietWeb.Core.Models;
using DietWeb.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic; // נחוץ עבור List<string>
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")] // ה-URL הראשי הוא /api/Recipe
public class RecipeController : ControllerBase
{
    private readonly IRecipeService _service;

    public RecipeController(IRecipeService service)
    {
        _service = service;
    }

    // נסיר את כל המתודות של ה-CRUD שהיו כאן:
    // [HttpGet] GetAll()
    // [HttpGet("{id}")] GetById(int id)
    // [HttpPost] Create(Recipe recipe)
    // [HttpPut("{id}")] Update(int id, Recipe recipe)
    // [HttpDelete("{id}")] Delete(int id)

    // המתודה היחידה שתשאר: חיפוש מתכון מ-AI
    [HttpGet("searchAI")] // ה-URL יהיה /api/Recipe/searchAI?query=...
    public async Task<ActionResult<Recipe>> SearchAIRecipe(
        [FromQuery] string query,
        [FromQuery] bool vegetarian = false,
        [FromQuery] bool vegan = false,
        [FromQuery] bool glutenFree = false,
        [FromQuery] bool dairyFree = false)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            // חשוב: החזר אובייקט Recipe תקין עם הודעת שגיאה
            return BadRequest(new Recipe
            {
                Title = "שגיאה: חסר שם מתכון",
                Description = "אנא הזן מילת מפתח למתכון שברצונך לחפש.",
                Ingredients = new List<string>(),
                Instructions = new List<string>(),
                ReadyInMinutes = null,
                Servings = null,
                Vegetarian = vegetarian,
                Vegan = vegan,
                GlutenFree = glutenFree,
                DairyFree = dairyFree,
                Image = "https://via.placeholder.com/300x200?text=Missing+Query",
                Likes = 0
            });
        }

        try
        {
            var recipe = await _service.GetRecipeFromAIAsync(query, vegetarian, vegan, glutenFree, dairyFree);

            // המתכון כבר מגיע מה-Service עם כותרת "שגיאה" אם הייתה בעיה ב-AI
            if (recipe.Title.Contains("שגיאה") || recipe.Title.Contains("לא נמצא/נוצר"))
            {
                // במקרה של שגיאה שמוחזרת מה-Service, נחזיר אותה עם סטטוס 500
                return StatusCode(500, recipe);
            }

            // במקרה של הצלחה
            return Ok(recipe);
        }
        catch (Exception ex)
        {
            // לטפל בשגיאות בלתי צפויות מה-Service (לדוגמה, אם ה-OpenAI API Key שגוי או בעיות רשת)
            Console.WriteLine($"Error in controller calling AI service: {ex.Message}");
            return StatusCode(500, new Recipe
            {
                Title = "שגיאה פנימית בשרת",
                Description = $"אירעה שגיאה בלתי צפויה: {ex.Message}. אנא נסה שוב מאוחר יותר.",
                Ingredients = new List<string>(),
                Instructions = new List<string>(),
                ReadyInMinutes = null,
                Servings = null,
                Vegetarian = vegetarian,
                Vegan = vegan,
                GlutenFree = glutenFree,
                DairyFree = dairyFree,
                Image = "https://via.placeholder.com/300x200?text=Internal+Error",
                Likes = 0
            });
        }
    }
}