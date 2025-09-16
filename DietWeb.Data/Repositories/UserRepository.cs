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
                .Include(u => u.DietaryPreferences)
                .Include(u => u.FavoriteRecipes)
                .ThenInclude(fr=>fr.Recipe)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task UpdateFavoriteRecipesAsync(int userId, Recipe recipes)
        {
            Console.WriteLine("Updating user ID: " + userId);

            var existingUser = await _context.Users
                .Include(u => u.FavoriteRecipes)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (existingUser == null)
            {
                Console.WriteLine("User not found.");
                return;
            }
            //// ✅ תיקון חשוב: עדכון מתכונים מועדפים
            Console.WriteLine("Cleared existing recipes.");
            
                // ✅ תיקון: יצירת אובייקט חדש כדי שה-ID ייווצר על ידי המסד
                existingUser?.FavoriteRecipes?.Add(new FavoriteRecipe
                {
                    Recipe = recipes,
                    UserId = existingUser.Id
                });
                Console.WriteLine("Added recipe: " + recipes.Title);
            
            await _context.SaveChangesAsync();
            Console.WriteLine("Changes saved successfully!");
        }


        public async Task UpdateAsync(User user)
        {
            Console.WriteLine("Updating user ID: " + user.Id);

            var existingUser = await _context.Users
                .Include(u => u.DietaryPreferences)
                .Include(u => u.FavoriteRecipes)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (existingUser == null)
            {
                Console.WriteLine("User not found.");
                return;
            }

            // עדכון שדות רגילים ידנית
            existingUser.currentWeight = user.currentWeight;
            existingUser.GoalWeight = user.GoalWeight;
            existingUser.Height = user.Height;
            existingUser.ChatPersonality = user.ChatPersonality;
            existingUser.ProgramLevel = user.ProgramLevel;

            // עדכון העדפות תזונתיות
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

            //// ✅ תיקון חשוב: עדכון מתכונים מועדפים
            //if (user.FavoriteRecipes != null)
            //{
            //    Console.WriteLine("Received FavoriteRecipes count: " + user.FavoriteRecipes.Count);
            //    existingUser?.FavoriteRecipes?.Clear();
            //    Console.WriteLine("Cleared existing recipes.");

            //    foreach (var recipe in user.FavoriteRecipes)
            //    {
            //        // ✅ תיקון: יצירת אובייקט חדש כדי שה-ID ייווצר על ידי המסד
            //        existingUser?.FavoriteRecipes?.Add(new FavoriteRecipe
            //        {
            //            Title = recipe.Title,
            //            UserId = existingUser.Id
            //        });
            //        Console.WriteLine("Added recipe: " + recipe.Title);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("No FavoriteRecipes received.");
            //}

            await _context.SaveChangesAsync();
            Console.WriteLine("Changes saved successfully!");
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

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
        public async Task<User?> GetUserWithFavoriteRecipes(int userId)
        {
            return await _context.Users
                .Include(u => u.FavoriteRecipes)
         .ThenInclude(fr => fr.Recipe) 

                .FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}