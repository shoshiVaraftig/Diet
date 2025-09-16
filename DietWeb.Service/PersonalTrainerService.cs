using DietWeb.Core.Services;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;
using DietWeb.Core.Models;
using DietWeb.Data;
using DietWeb.Core.Repositories;
using Microsoft.EntityFrameworkCore;





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

            _client = new OpenAIClient(new OpenAIAuthentication(apiKey));
            _db = db;
            _repo = repo;
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

            
            var history = await _db.Messages
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.Timestamp) // Order by latest first
                .Take(5) // Take only the last 10 messages
                .OrderBy(m => m.Timestamp) // Re-order back to chronological order
                .ToListAsync();


            var messages = new List<Message>
{
    new(Role.System, $"אתה מאמן תזונה אישי בשם 'אורי'. האופי שלך הוא {{personality}}. ענה על שאלות רק בנושא תזונה בריאה. אם המשתמש שואל על נושא אחר, ענה בנימוס שאינך יכול לענות על כך.\r\n    \r\n    תפקידך הוא לספק ליווי וייעוץ בנושאי תזונה, הרגלי אכילה ובריאות כללית הקשורה לאוכל.\r\n    תשובותיך צריכות להיות קצרות, תמציתיות, ללא שגיאות כתיב ובעברית בלבד. הניסוח צריך להיות ברור ומקצועי.\r\n\r\n    כאשר המשתמש שואל שאלה כללית (לדוגמה, 'מה כדאי לאכול בצהריים?'), אל תספק רשימה של מתכונים או רעיונות כלליים.\r\n    במקום זאת, עליך לנהל שיחה קצרה על מנת להבין את הצרכים הספציפיים שלו. שאל אותו שאלות מנחות כמו: 'אילו מצרכים יש לך בבית?' או 'אילו מאכלים אתה מעדיף לאכול?'.\r\n\r\n    לאחר שתקבל את המידע, ספק תשובה ספציפית וקצרה שמבוססת על המידע שקיבלת מהמשתמש. הימנע ממתן תשובות רשימתיות או כלליות. ענה כאילו אתה אדם אמיתי שמנהל שיחה ולא בוט שנותן תשובה קבועה")
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
