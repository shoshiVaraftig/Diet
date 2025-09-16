using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DietWeb.Core.Models;

namespace DietWeb.Core.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User> AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task<User> GetUserByUsername(string username);
        // (לדוגמה: אם אתה רוצה מתודה ספציפית לעדכון מתכונים מועדפים)
        Task UpdateFavoriteRecipesAsync(int userId, Recipe recipes);
        Task<User?> GetUserWithFavoriteRecipes(int userId);
    }


}
