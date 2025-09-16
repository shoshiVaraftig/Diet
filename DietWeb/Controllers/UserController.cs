using System.Text.Json;
using DietWeb.API.DTOs;
using DietWeb.Core.Models;
using DietWeb.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var users = await _service.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _service.GetByIdAsync(id);
        return user == null ? NotFound() : Ok(user);
    }


    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateFavorite(int id, Recipe recipes)
    {
        await _service.UpdateFavoriteRecipesAsync(id, recipes);
        //return Ok();
        return NoContent();

    }

    [HttpPatch("{id}")]
    [Authorize]
    public async Task<IActionResult> PartialUpdate(int id, [FromBody] UpdateUserDto update)
    {
        Console.WriteLine("📥 Payload שהתקבל:");
        Console.WriteLine(JsonSerializer.Serialize(update));

        var user = await _service.GetUserWithFavoriteRecipes(id);
        if (user == null) return NotFound();

        // ✅ עדכון שדות בסיסיים
        if (update.CurrentWeight.HasValue)
        {
            if (user.StartWeight == null)
                user.StartWeight = update.CurrentWeight.Value;
            user.currentWeight = update.CurrentWeight.Value;
        }
        if (update.Height.HasValue)
            user.Height = update.Height.Value;
        if (update.GoalWeight.HasValue)
            user.GoalWeight = update.GoalWeight.Value;
        if (!string.IsNullOrEmpty(update.ProgramLevel))
            user.ProgramLevel = update.ProgramLevel;
        if (!string.IsNullOrEmpty(update.ChatPersonality))
            user.ChatPersonality = update.ChatPersonality;

        

        await _service.UpdateAsync(user);
        return Ok(update);

    }



}
