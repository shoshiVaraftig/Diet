using DietWeb.Core.Models;
using DietWeb.Core.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class RecipeController : ControllerBase
{
    private readonly IRecipeService _service;
    public RecipeController(IRecipeService service)
    {
        _service = service;
    }


    [HttpPost("generate")]
    public async Task<IActionResult> GenerateRecipe([FromBody] GenerateRecipeRequest request)
    {
        // הקריאה לפונקציית יצירת המתכון באמצעות AI
        var recipe = await _service.GenerateRecipeAsync(
            request.Query,
            request.IsVegetarian,
            request.IsVegan,
            request.IsGlutenFree,
            request.IsDairyFree
        );
        // החזרת האובייקט ישירות ללקוח
        return Ok(recipe);
    }
}
