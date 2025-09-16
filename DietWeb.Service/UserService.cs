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
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<User>> GetAllAsync() => _repository.GetAllAsync();
        public Task<User?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
        public Task<User> AddAsync(User user) => _repository.AddAsync(user);
        public Task UpdateAsync(User user) => _repository.UpdateAsync(user);
        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);

         public async Task<User> GetUserByUsername(string username)
        {
            return await _repository.GetUserByUsername(username);
        }

        public Task UpdateFavoriteRecipesAsync(int userId, Recipe recipe)
        {
            return _repository.UpdateFavoriteRecipesAsync(userId, recipe);
        }
        public async Task<User?> GetUserWithFavoriteRecipes(int userId)
        {
            // אתה צריך לוודא ששליפת המשתמש כוללת את רשימת המתכונים
            return await _repository.GetUserWithFavoriteRecipes(userId);
        }
    }

}
