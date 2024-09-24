using MessageHub.Domain.Entities;
using MessageHub.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MessageHub.Infrastructure.Repositories
{
    public class PushTokenRepository : BaseRepository<PushToken>
    {
        public PushTokenRepository(MessageDbContext context) : base(context)
        {

        }

        public async Task<PushToken> GetByValueAsync(string account, string value)
        {
            return await _context.PushTokens.FirstOrDefaultAsync(t => t.Account == account && t.Value == value);
        }

        public List<PushToken> GetByExternalClient(string externalClientID, string senderCode)
        {
            return _context.PushTokens.Where(t => t.ExternalClientID == externalClientID).ToList();
        }

        public async Task<List<PushToken>> GetValidByExternalClientAsync(string externalClientID, string account, string senderCode)
        {
            return await _context.PushTokens
                .Where(t => t.ExternalClientID == externalClientID
                && t.ExpirationDate == null
                && t.Account == account).ToListAsync();
        }
    }
}
