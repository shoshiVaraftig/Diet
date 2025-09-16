using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DietWeb.Core.Models;
using DietWeb.Core.Repositories;
using DietWeb.Core.Services;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;

namespace DietWeb.Service
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _repository;
        private readonly OpenAIClient _client;

        public RecipeService(IRecipeRepository repository, IConfiguration config)
        {
            _repository = repository;

            // יצירת לקוח OpenAI עם המפתח מהגדרות האפליקציה
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? config["OpenAI:ApiKey"];
            _client = new OpenAIClient(new OpenAIAuthentication(apiKey));
        }

        public Task<IEnumerable<Recipe>> GetAllAsync() => _repository.GetAllAsync();
        public Task<Recipe?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
        public Task<Recipe> AddAsync(Recipe recipe) => _repository.AddAsync(recipe);
        public Task UpdateAsync(Recipe recipe) => _repository.UpdateAsync(recipe);
        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);

        public async Task<Recipe> GenerateRecipeAsync(string query, bool isVegetarian, bool isVegan, bool isGlutenFree, bool isDairyFree)
        {
            try
            {
                // 1. בניית הפרומפט (Prompt) לשירות ה-AI
                var prompt = BuildRecipePrompt(query, isVegetarian, isVegan, isGlutenFree, isDairyFree);

                // 2. שליחת הבקשה ל-OpenAI
                var request = new ChatRequest(
                    messages: new[] { new Message(Role.User, prompt) },
                    model: "gpt-4-turbo",
                    responseFormat: ChatResponseFormat.Json
                );

                var result = await _client.ChatEndpoint.GetCompletionAsync(request);
                var jsonResponse = result.FirstChoice.Message.Content.ToString().Trim();

                // 3. ניתוח התשובה מה-AI והמרתה לאובייקט C#
                var generatedRecipe = JsonSerializer.Deserialize<Recipe>(jsonResponse, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (generatedRecipe == null || string.IsNullOrEmpty(generatedRecipe.Title))
                {
                    throw new Exception("התשובה מה-AI אינה תקינה או חסרה נתונים.");
                }

                // עדכון שדות בוליאניים לפי הבקשה המקורית
                generatedRecipe.Vegetarian = isVegetarian;
                generatedRecipe.Vegan = isVegan;
                generatedRecipe.GlutenFree = isGlutenFree;
                generatedRecipe.DairyFree = isDairyFree;

                return generatedRecipe;
            }
            catch (Exception ex)
            {
                // טיפול בשגיאות מה-AI או בניתוח הנתונים
                throw new Exception($"שגיאה ביצירת המתכון: {ex.Message}");
            }
        }

        /// <summary>
        /// בונה את הפרומפט לשירות ה-AI עם הנחיות ספציפיות.
        /// </summary>
        private string BuildRecipePrompt(string query, bool isVegetarian, bool isVegan, bool isGlutenFree, bool isDairyFree)
        {
            var filters = new List<string>();
            if (isVegetarian) filters.Add("צמחוני");
            if (isVegan) filters.Add("טבעוני");
            if (isGlutenFree) filters.Add("ללא גלוטן");
            if (isDairyFree) filters.Add("ללא חלב");

            var filterString = filters.Any() ? $" ודא שהמתכון עונה על הדרישות הבאות: {string.Join(", ", filters)}." : "";

            // ההנחיה המרכזית שמנחה את ה-AI להחזיר JSON מסודר ובעברית
            return $@"
                חפש בגוגל והעתק מתכון ישראלי למנה בשם '{query}' בפורמט JSON בלבד. 
                {filterString}
                האובייקט צריך להיות במבנה הבא בעברית:
                {{
                    ""Title"": ""שם המתכון"",
                    ""Description"": ""תיאור קצר ומפתה"",
                 ""ReadyInMinutes"": 0,
                    ""Servings"": 0,
                    ""Ingredients"": [""מרכיב 1"", ""מרכיב 2""],
                    ""Instructions"": [""שלב 1"", ""שלב 2""],
                    ""Image"": ""כתובת לתמונה רלוונטית"",
                    ""Likes"": 0
                }}
            ";
        }
    }
}