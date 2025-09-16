using DietWeb.Core.Models;


namespace DietWeb.Core.Services
{
    public interface IFoodService
    {
        Task<Food> GenerateFoodItemAsync(string query);

        //Task<IEnumerable<Food>> GetAllAsync();
        //Task<Food?> GetByIdAsync(int id);
        //Task<Food> AddAsync(Food food);44111
        //Task UpdateAsync(Food food);
        //Task DeleteAsync(int id);
        //Task<Food> GetFoodByNameAsync(string foodName);
    }

}
