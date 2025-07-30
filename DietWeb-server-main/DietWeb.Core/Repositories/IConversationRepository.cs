using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DietWeb.Core.Models;

namespace DietWeb.Core.Repositories
{
    public interface IConversationRepository
    {
        Task<List<ConversationMessage>> GetMessagesByUserAsync(string userId);
    }

}
