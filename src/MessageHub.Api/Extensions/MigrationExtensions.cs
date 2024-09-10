﻿using MessageHub.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace MessageHub.Api.Extensions
{
    internal static class MigrationExtensions
    {
        internal static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            ApplyMigration<MessageDbContext>(scope);
        }

        private static void ApplyMigration<TDbContext>(IServiceScope scope)
            where TDbContext : DbContext
        {
            using TDbContext context = scope.ServiceProvider.GetRequiredService<TDbContext>();
            context.Database.Migrate();
        }
    }
}