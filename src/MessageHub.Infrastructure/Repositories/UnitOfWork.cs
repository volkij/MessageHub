using MessageHub.Infrastructure.Database;

namespace MessageHub.Infrastructure.Repositories
{
    public class UnitOfWork
    {
        //https://www.linkedin.com/pulse/implementing-unit-work-repository-pattern-net-core-bany-elmarjeh-es5cf/

        private readonly MessageDbContext _context;

        public UnitOfWork(MessageDbContext context)
        {
            _context = context;

            MessageRepository = new MessageRepository(context);
            PushTokenRepository = new PushTokenRepository(context);
            TemplateRepository = new TemplateRepository(context);
        }

        public MessageRepository MessageRepository { get; }

        public TemplateRepository TemplateRepository { get; }

        public PushTokenRepository PushTokenRepository { get; }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}