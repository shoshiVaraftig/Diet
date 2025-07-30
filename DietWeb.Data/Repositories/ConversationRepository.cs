using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DietWeb.Data.Repositories
{
    using DietWeb.Core.Models;
    using DietWeb.Core.Repositories;
    using Microsoft.EntityFrameworkCore;

    public class ConversationRepository : IConversationRepository
    {
        private readonly DataContext _context;

        public ConversationRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<List<ConversationMessage>> GetMessagesByUserAsync(string userId)
        {
            return await _context.Messages
                .Where(m => m.UserId == userId)
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }
    }

}
