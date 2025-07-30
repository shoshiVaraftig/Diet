using DietWeb.Core.Models;
using DietWeb.Core.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DietWeb.Core.Services
{
    public interface IRecipeService
    {

        Task<Recipe> GetRecipeFromAIAsync(string query, bool vegetarian, bool vegan, bool glutenFree, bool dairyFree);

    }

}

