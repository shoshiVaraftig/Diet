using DietWeb.Core.Models;
using DietWeb.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class FoodController : ControllerBase
{
    private readonly IFoodService _service;

    public FoodController(IFoodService service)
    {
        _service = service;
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateFoodItem([FromBody] FoodItemRequest request)
    {
        var foodItem = await _service.GenerateFoodItemAsync(request.Query);
        return Ok(foodItem);
    }
}