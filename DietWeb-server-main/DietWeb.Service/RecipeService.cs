using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DietWeb.Core.Models;
// נסיר את DietWeb.Core.Repositories;
using DietWeb.Core.Services;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace DietWeb.Service
{
    public class RecipeService : IRecipeService
    {
        // נסיר את private readonly IRecipeRepository _repository;
        private readonly OpenAIClient _openAiClient;
        private readonly IConfiguration _configuration;

        // הקונסטרוקטור יקבל רק IConfiguration
        public RecipeService(IConfiguration configuration)
        {
            _configuration = configuration;
            _openAiClient = new OpenAIClient(new OpenAIAuthentication(_configuration["OpenAI:ApiKey"]));
        }

        // נסיר את כל המתודות של ה-CRUD: GetAllAsync, GetByIdAsync, AddAsync, UpdateAsync, DeleteAsync

        // יישום מתודת חיפוש מ-AI
        public async Task<Recipe> GetRecipeFromAIAsync(string query, bool vegetarian, bool vegan, bool glutenFree, bool dairyFree)
        {
            var chatPrompts = new List<Message>
            {
                new Message(Role.System, "אתה שף מומחה ויוצר מתכונים. המטרה שלך היא לספק מתכונים בריאים ומפורטים בפורמט JSON בלבד. ודא שהמתכון הוא אמיתי, הגיוני, וניתן להכנה. אל תכלול שום טקסט מחוץ ל-JSON. המתכון צריך להיות בפורמט JSON הבא: {\"Title\": \"שם המתכון\", \"Description\": \"תיאור קצר\", \"Ingredients\": [\"מרכיב 1\", \"מרכיב 2\"], \"Instructions\": [\"שלב 1\", \"שלב 2\"], \"ReadyInMinutes\": זמן הכנה בדקות (מספר), \"Servings\": מספר מנות (מספר), \"Vegetarian\": true/false, \"Vegan\": true/false, \"GlutenFree\": true/false, \"DairyFree\": true/false, \"Image\": \"כתובת URL לתמונה גנרית של האוכל (לא חובה, אבל מומלץ)\", \"Likes\": 0}"),
                new Message(Role.System, "אם בקשת המשתמש אינה למתכון, ענה עם JSON ריק או מתכון גנרי המציין שזו אינה בקשה למתכון, אך עדיין שמור על פורמט ה-JSON."),
                new Message(Role.System, "הקפד שהתשובה תמיד תהיה ב-JSON תקין וקריא, גם אם המתכון אינו מפורט מאוד. עבור Image, חפש 'Unsplash' או 'Pexels' ובחר תמונה גנרית ורלוונטית. לדוגמה, עבור פסטה, חפש 'pasta dish' וקח URL של תמונה כלשהי. התמונה חייבת להיות קישור ישיר תקין."),
            };

            var userQuery = $"תן לי מתכון ל'{query}'. ";

            if (vegetarian) userQuery += "צמחוני. ";
            if (vegan) userQuery += "טבעוני. ";
            if (glutenFree) userQuery += "ללא גלוטן. ";
            if (dairyFree) userQuery += "ללא מוצרי חלב. ";

            userQuery += "המתכון צריך להיות בפורמט JSON בלבד, כפי שהוגדר בהנחיות המערכת.";

            chatPrompts.Add(new Message(Role.User, userQuery));

            try
            {
                var chatRequest = new ChatRequest(chatPrompts, Model.GPT3_5_Turbo);
                var response = await _openAiClient.ChatEndpoint.GetCompletionAsync(chatRequest);

                if (response?.Choices == null || response.Choices.Count == 0 || response.Choices[0].Message?.Content == null)
                {
                    Console.WriteLine("AI did not return valid content. Returning a default empty recipe.");
                    return GetDefaultEmptyRecipe(query, vegetarian, vegan, glutenFree, dairyFree);
                }

                var aiResponseContent = response.Choices[0].Message.Content;
                Console.WriteLine($"AI Raw Response: {aiResponseContent}");

                aiResponseContent = aiResponseContent.Replace("```json", "").Replace("```", "").Trim();

                Recipe? aiRecipe = JsonSerializer.Deserialize<Recipe>(aiResponseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (aiRecipe == null)
                {
                    Console.WriteLine("Failed to deserialize AI response into Recipe. Returning a default empty recipe.");
                    return GetDefaultEmptyRecipe(query, vegetarian, vegan, glutenFree, dairyFree);
                }

                //


                aiRecipe.Vegetarian = vegetarian;
                aiRecipe.Vegan = vegan;
                aiRecipe.GlutenFree = glutenFree;
                aiRecipe.DairyFree = dairyFree;

                if (string.IsNullOrWhiteSpace(aiRecipe.Image) || !Uri.TryCreate(aiRecipe.Image, UriKind.Absolute, out Uri? uriResult) || !(uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                {
                    aiRecipe.Image = "https://via.placeholder.com/300x200?text=Recipe+Image";
                }

                if (aiRecipe.Id == 0) aiRecipe.Id = new Random().Next(1, 1000000);
                aiRecipe.Likes = 0;

                return aiRecipe;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting recipe from AI: {ex.Message}");
                return GetDefaultEmptyRecipe(query, vegetarian, vegan, glutenFree, dairyFree);
            }
        }

        private Recipe GetDefaultEmptyRecipe(string query, bool vegetarian, bool vegan, bool glutenFree, bool dairyFree)
        {
            return new Recipe
            {
                Id = new Random().Next(1, 1000000),
                Title = $"מתכון ל'{query}' לא נמצא/נוצר כרגע.",
                Description = "אירעה שגיאה בקבלת המתכון או שהבקשה לא הייתה ברורה מספיק. נסה בבקשה ניסוח אחר.",
                Ingredients = new List<string> { "אין מרכיבים זמינים" },
                Instructions = new List<string> { "אין הוראות זמינות" },
                ReadyInMinutes = null,
                Servings = null,
                Vegetarian = vegetarian,
                Vegan = vegan,
                GlutenFree = glutenFree,
                DairyFree = dairyFree,
                Image = "https://via.placeholder.com/300x200?text=No+Recipe+Found",
                Likes = 0
            };
        }
    }
}