using MessageHub.Domain.Entities;
using MessageHub.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MessageHub.Infrastructure.Repositories
{
    public class TemplateRepository : BaseRepository<Template>
    {
        public TemplateRepository(MessageDbContext context) : base(context)
        {

        }
        public Template GetTemplateByCode(string code)
        {
            return _context.Templates.FirstOrDefault(t => t.Code == code);
        }

        public async Task<List<Template>> GetAllTemplates()
        {
            return await _context.Templates.ToListAsync();
        }
    }
}
