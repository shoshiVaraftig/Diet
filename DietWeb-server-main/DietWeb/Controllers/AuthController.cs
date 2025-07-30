// Controllers/AuthController.cs
using DietWeb.Core.Models;
using DietWeb.Core.Services;
using DietWeb.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService; // אולי נזדקק לו עבור קריאה/יצירה של משתמשים

    public AuthController(IAuthService authService, IUserService userService) // הזרקת ה-AuthService
    {
        _authService = authService;
        _userService = userService;
    }

    // נקודת קצה להרשמה
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        // בדיקה אם שם המשתמש כבר קיים
        var existingUser = await _userService.GetUserByUsername(request.Username); // וודא ש-UserService מכיל מתודה כזו
        if (existingUser != null)
        {
            return BadRequest("Username already exists.");
        }

        var newUser = new User
        {
            Username = request.Username,
            Email = request.Email, // <--- הוסף שורה זו!
            // HashedPassword יוזן ע"י ה-AuthService
        };

        var registeredUser = await _authService.Register(newUser, request.Password);


        // Check for successful registration from AuthService
        // Assuming _authService.Register returns the User object or a success indicator
        if (registeredUser == null) // Or if your AuthService returns a Result object and it's not successful
        {
            return BadRequest("Registration failed, user not created."); // Add appropriate error handling
        }

        // Generate a JWT token for the newly registered user
        var token = _authService.GenerateJwtToken(registeredUser); // Assuming this method exists and takes a User object
        return CreatedAtAction(nameof(Register), new { id = registeredUser.Id }, new { registeredUser.Id, registeredUser.Username, registeredUser.Email, Token = token });

    }

    // נקודת קצה להתחברות
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var user = await _authService.ValidateUserCredentials(request.Username, request.Password);

        if (user == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        var token = _authService.GenerateJwtToken(user);

        return Ok(new { Message = "Logged in", Token = token, ID=user.Id });
    }
}