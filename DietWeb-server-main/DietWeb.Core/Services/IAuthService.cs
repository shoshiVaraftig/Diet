// Core/Services/IAuthService.cs
using DietWeb.API.DTOs;
using DietWeb.Core.Models;
using System.Threading.Tasks;

namespace DietWeb.Core.Services
{
    public interface IAuthService
    {
        Task<User> Register(User user, string password);
        Task<User> ValidateUserCredentials(string username, string password);
        string GenerateJwtToken(User user);
        Task<LoginResponse?> LoginAsync(string username, string password);
    }
}