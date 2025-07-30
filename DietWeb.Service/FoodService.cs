

using DietWeb.Core.Models;
using DietWeb.Core.Repositories;
using DietWeb.Core.Services;

namespace DietWeb.Service
{
    public class FoodService : IFoodService
    {
        private readonly IFoodRepository _repository;

        public FoodService(IFoodRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Food>> GetAllAsync() => _repository.GetAllAsync();


        public async Task<Food> GetFoodByNameAsync(string foodName) // חייב להיות async Task<Food>
        {
            var food = await _repository.GetFoodByNameAsync(foodName);

            if (food == null)
            {
                // חשוב לטפל במקרה שלא נמצא מזון
                throw new KeyNotFoundException($"Food '{foodName}' not found.");
            }

            return food;
        }



        public Task<Food?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task<Food> AddAsync(Food food) => _repository.AddAsync(food);

        public Task UpdateAsync(Food food) => _repository.UpdateAsync(food);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }

}
