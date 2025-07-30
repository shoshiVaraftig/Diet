using DietWeb.Core.Models;
using DietWeb.Core.Services;
using DietWeb.Data;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PersonalTrainerController : ControllerBase
{
    private readonly IPersonalTrainerService _trainerService;

 

    [HttpPost("ask")]
    public async Task<IActionResult> AskTrainer([FromBody] TrainerRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Personality) ||
            string.IsNullOrWhiteSpace(request.Input) ||
            string.IsNullOrWhiteSpace(request.UserId))
        {
            return BadRequest("Missing required fields.");
        }

        var response = await _trainerService.GetTrainerResponseAsync(
            request.Personality, request.Input, request.UserId);

        return Ok(new { answer = response });
    }

    private readonly DataContext _context;

    public PersonalTrainerController(IPersonalTrainerService trainerService, DataContext context)
    {
        _trainerService = trainerService;
        _context = context;
    }


    [HttpGet("history/{userId}")]
    public async Task<IActionResult> GetHistory(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return BadRequest("User ID is required.");

        var history = await _trainerService.GetConversationHistoryAsync(userId);

        var result = history.Select(m => new
        {
            m.Role,
            m.Content,
            m.Timestamp
        });

        return Ok(result);
    }


}

