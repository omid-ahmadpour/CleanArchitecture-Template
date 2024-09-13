using CleanTemplate.Persistence.Db;
using Microsoft.EntityFrameworkCore;

namespace CleanTemplate.Api
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;

    public interface IMigrationService
    {
        void ApplyMigrations();
    }

    public class MigrationService : IMigrationService
    {
        private readonly IServiceProvider _serviceProvider;

        public MigrationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void ApplyMigrations()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Apply Migration
            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                // Log the error or handle it in some way
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred while migrating the database.");
            }
        }
    }
}
