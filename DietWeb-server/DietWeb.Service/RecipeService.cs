using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DietWeb.Core.Models;
using DietWeb.Core.Repositories;
using DietWeb.Core.Services;

namespace DietWeb.Service
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _repository;

        public RecipeService(IRecipeRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Recipe>> GetAllAsync() => _repository.GetAllAsync();
        public Task<Recipe?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
        public Task<Recipe> AddAsync(Recipe recipe) => _repository.AddAsync(recipe);
        public Task UpdateAsync(Recipe recipe) => _repository.UpdateAsync(recipe);
        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }

}
