using DietWeb.Core.Models;
using DietWeb.Core.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class FoodController : ControllerBase
{
    private readonly IFoodService _foodService;

    public FoodController(IFoodService service)
    {
        _foodService = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var foods = await _foodService.GetAllAsync();
        return Ok(foods);
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchFoodByName([FromQuery] string foodName)
    {
        if (string.IsNullOrWhiteSpace(foodName))
        {
            return BadRequest("Food name cannot be empty.");
        }

        try
        {
            // כאן מצפים למאכל בודד
            var food = await _foodService.GetFoodByNameAsync(foodName);

            // אם המתודה GetFoodByNameAsync ב-FoodService זורקת KeyNotFoundException כשלא נמצא מזון:
            // אז לא צריך את בדיקת ה-null כאן, ה-catch יטפל בזה.
            return Ok(food);
        }
        catch (KeyNotFoundException ex)
        {
            // אם המאכל לא נמצא, מחזירים 404 (NotFound)
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            // טיפול בשגיאות כלליות אחרות
            return StatusCode(500, "An error occurred while searching for food: " + ex.Message);
        }
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var food = await _foodService.GetByIdAsync(id);
        return food == null ? NotFound() : Ok(food);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Food food)
    {
        var created = await _foodService.AddAsync(food);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Food food)
    {
        if (id != food.Id) return BadRequest();
        await _foodService.UpdateAsync(food);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _foodService.DeleteAsync(id);
        return NoContent();
    }
   
}

