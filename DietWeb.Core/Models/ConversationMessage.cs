using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietWeb.Core.Models
{
    public class ConversationMessage
    {
        public int Id { get; set; }
        public string UserId { get; set; } = "";
        public string Role { get; set; } = ""; // "user" or "assistant"
        public string Content { get; set; } = "";
        public DateTime Timestamp { get; set; }
    }

}
