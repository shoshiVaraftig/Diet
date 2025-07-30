using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DietWeb.Core.Models;

namespace DietWeb.Core.Services
{
    public interface IPersonalTrainerService
    {
        Task<string> GetTrainerResponseAsync(string personality, string input, string userId);
        Task<List<ConversationMessage>> GetConversationHistoryAsync(string userId);

    }


}
