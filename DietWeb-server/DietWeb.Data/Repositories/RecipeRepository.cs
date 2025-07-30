using Microsoft.EntityFrameworkCore;
using DietWeb.Core.Models;
using DietWeb.Core.Repositories;

namespace DietWeb.Data.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly DataContext _context;

        public RecipeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Recipe>> GetAllAsync() => await _context.Recipes.ToListAsync();

        public async Task<Recipe?> GetByIdAsync(int id) => await _context.Recipes.FindAsync(id);

        public async Task<Recipe> AddAsync(Recipe recipe)
        {
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            return recipe;
        }

        public async Task UpdateAsync(Recipe recipe)
        {
            _context.Entry(recipe).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();
            }
        }
    }

}
