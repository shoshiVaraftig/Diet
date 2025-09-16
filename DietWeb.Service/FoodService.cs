using DietWeb.Core.Models;
using DietWeb.Core.Services;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using System;
using System.Text.Json;
using System.Threading.Tasks;

public class FoodService : IFoodService
{
    private readonly OpenAIClient _client;

    // השתמש בבונה הזה בלבד
    public FoodService(OpenAIClient client)
    {
        _client = client;
    }


    public async Task<Food> GenerateFoodItemAsync(string query)
    {
        try
        {
            var prompt = BuildFoodItemPrompt(query);

            var request = new ChatRequest(
                messages: new[] { new Message(Role.User, prompt) },
                model: "gpt-4-turbo",
                responseFormat: ChatResponseFormat.Json
            );

            var result = await _client.ChatEndpoint.GetCompletionAsync(request);
            var jsonResponse = result.FirstChoice.Message.Content.ToString().Trim();

            var generatedFoodItem = JsonSerializer.Deserialize<Food>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (generatedFoodItem == null || string.IsNullOrEmpty(generatedFoodItem.Name))
            {
                throw new Exception("התשובה מה-AI אינה תקינה או חסרה נתונים.");
            }

            return generatedFoodItem;
        }
        catch (Exception ex)
        {
            throw new Exception($"שגיאה ביצירת פריט המזון: {ex.Message}");
        }
    }

    private string BuildFoodItemPrompt(string query)
    {
        return $@"
            צור אובייקט JSON בלבד על פריט מזון בשם '{query}'.
            עליך לספק את הנתונים הבאים בעברית:
            {{
                ""Name"": ""שם המזון"",
                ""Calories"": 0,
                ""Category"": ""קטגוריה"",
                ""ServingSize"": ""גודל מנה לדוגמה""

            }}
          .  ודא שכל המידע מדויק ככל הניתן. אם נשאלת על פריט שאינו מזון, תשיב: לא רלוונטי.
        ";
    }
}