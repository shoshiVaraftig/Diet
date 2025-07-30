using Microsoft.EntityFrameworkCore;
using DietWeb.Core.Models;
using DietWeb.Core.Repositories;
using System.Linq;
namespace DietWeb.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync() => await _context.Users.ToListAsync();

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.DietaryPreferences) // ← כאן הפואנטה
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            var existingUser = await _context.Users
                .Include(u => u.DietaryPreferences)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existingUser == null) return;

            // ✅ עדכון שדות רגילים ידנית
            existingUser.currentWeight = user.currentWeight;
            existingUser.GoalWeight = user.GoalWeight;
            existingUser.Height = user.Height;
            existingUser.ChatPersonality = user.ChatPersonality;
            existingUser.ProgramLevel = user.ProgramLevel;

            // ✅ עדכון העדפות תזונתיות
            if (user.DietaryPreferences != null)
            {
                existingUser?.DietaryPreferences?.Clear();

                foreach (var pref in user.DietaryPreferences)
                {
                    existingUser?.DietaryPreferences?.Add(new DietaryPreference
                    {
                        FoodName = pref.FoodName,
                        Like = pref.Like,
                        UserId = pref.UserId
                    });
                }
            }

            await _context.SaveChangesAsync(); // ✅ שמירה בפועל
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
        // *** מימוש המתודה החדשה ***
        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }

}
