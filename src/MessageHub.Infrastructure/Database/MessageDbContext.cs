using MessageHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MessageHub.Infrastructure.Database
{
    public class MessageDbContext(DbContextOptions<MessageDbContext> options) : DbContext(options)
    {
        internal DbSet<Message> Messages { get; set; }
        internal DbSet<MessageAttribute> MessageAttributes { get; set; }
        internal DbSet<Domain.Entities.MessageAttachment> MessageAttachments { get; set; }

        internal DbSet<PushToken> PushTokens { get; set; }

        internal DbSet<Template> Templates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schemas.Messages);
        }
    }
}