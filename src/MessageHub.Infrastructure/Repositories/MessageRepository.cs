using MessageHub.Domain.Entities;
using MessageHub.Infrastructure.Database;
using MessageHub.Shared;
using Microsoft.EntityFrameworkCore;

namespace MessageHub.Infrastructure.Repositories
{
    public class MessageRepository(MessageDbContext context) : BaseRepository<Message>(context)
    {
        public async Task<List<Message>> GetMessagesOlderThanAsyn(DateTime dateTo)
        {
            return await _context.Messages.Where(m => m.CreateDate < dateTo).ToListAsync();
        }

        public async Task<List<Message>> GetMessagesByTypeAndExternalClientAsync(MessageType messageType, string externalClientID)
        {
            return await _context.Messages
                                 .Where(m => m.Type == messageType.ToString() && m.ExternalClientID == externalClientID)
                                 .ToListAsync();
        }
    }
}