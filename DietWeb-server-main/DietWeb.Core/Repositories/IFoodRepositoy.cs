using DietWeb.Core.Models;

namespace DietWeb.Core.Repositories
{
    public interface IFoodRepository
    {
        Task<IEnumerable<Food>> GetAllAsync();
        Task<Food?> GetByIdAsync(int id);
        Task<Food> AddAsync(Food food);
        Task UpdateAsync(Food food);
        Task DeleteAsync(int id);
        Task<Food> GetFoodByNameAsync(string foodName);
    }

}
