

using DietWeb.Core.Models;
using DietWeb.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DietWeb.Data.Repositories
{
    public class FoodRepository : IFoodRepository
    {
        private readonly DataContext _context;

        public FoodRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Food>> GetAllAsync() => await _context.Foods.ToListAsync();

        public async Task<Food> GetFoodByNameAsync(string foodName)
        {
            var food = await _context.Foods.FirstOrDefaultAsync(f => f.Name == foodName);
            return food;
        }
        public async Task<Food> GetFoodByNameAsync2(string foodName) // <--- ודא שזו קיימת כאן
        {
            var food = await _context.Foods.FirstOrDefaultAsync(f => f.Name == foodName);
            return food;
         }

        public async Task<Food?> GetByIdAsync(int id) => await _context.Foods.FindAsync(id);

        public async Task<Food> AddAsync(Food food)
        {
            _context.Foods.Add(food);
            await _context.SaveChangesAsync();
            return food;
        }

        public async Task UpdateAsync(Food food)
        {
            _context.Entry(food).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var food = await _context.Foods.FindAsync(id);
            if (food != null)
            {
                _context.Foods.Remove(food);
                await _context.SaveChangesAsync();
            }
        }
    }

}
