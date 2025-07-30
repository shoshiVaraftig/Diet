using DietWeb.Core.Services;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using DietWeb.Core.Models;
using DietWeb.Data;
using DietWeb.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions; // הוסף את זה אם לא קיים





namespace DietWeb.Service
{

    public class PersonalTrainerService : IPersonalTrainerService
    {
        private readonly OpenAIClient _client;
        private readonly DataContext _db;

        private readonly IConversationRepository _repo;

        public PersonalTrainerService(IConfiguration config, DataContext db, IConversationRepository repo)
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                         ?? config["OpenAI:ApiKey"];
             }
           
        public async Task<List<ConversationMessage>> GetConversationHistoryAsync(string userId)
        {
            return await _repo.GetMessagesByUserAsync(userId);
        }

        public async Task<string> GetTrainerResponseAsync(string personality, string input, string userId)
        {
            // Save user message
            var userMessage = new ConversationMessage
            {
                UserId = userId,
                Role = "user",
                Content = input,
                Timestamp = DateTime.UtcNow
            };

            _db.Messages.Add(userMessage);
            await _db.SaveChangesAsync();

            // Get conversation history
            var history = await _db.Messages
                .Where(m => m.UserId == userId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            var messages = new List<Message>
        {
            new(Role.System, $"אתה מאמן תזונה יש לענות תשובות על פי הסגנון : {personality}"),
            new(Role.System, "אתה עוזר אישי לתזונה בלבד. תענה רק על שאלות שקשורות לתזונה, הרגלי אכילה, ליווי תזונתי ובריאות הקשורה לתזונה. אל תענה על שאלות שאינן קשורות לתחום זה."),
            new(Role.System, "חשוב: כל ייעוץ או מידע שתספק אינו מהווה תחליף לייעוץ רפואי, אבחון, טיפול או הנחיה תזונתית מקצועית מרופא או דיאטן. עליך להפנות את המשתמשים להתייעץ תמיד עם גורם מקצועי מוסמך לפני קבלת החלטות הנוגעות לבריאותם או לתזונתם."),

        };

            messages.AddRange(history.Select(m =>
                new Message(m.Role == "user" ? Role.User : Role.Assistant, m.Content)
            ));

            var request = new ChatRequest(messages, model: "gpt-3.5-turbo");

            var result = await _client.ChatEndpoint.GetCompletionAsync(request);
            var reply = result.FirstChoice.Message.Content.ToString().Trim();
            
            // Save assistant reply
            var assistantMessage = new ConversationMessage
            {
                UserId = userId,
                Role = "assistant",
                Content = reply,
                Timestamp = DateTime.UtcNow
            };
            _db.Messages.Add(assistantMessage);
            await _db.SaveChangesAsync();

            return reply;
        }

    }




}
