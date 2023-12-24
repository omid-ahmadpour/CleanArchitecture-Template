using System.Reflection;

namespace CleanTemplate.Persistence
{
    using Common.General;
    using Db;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            var appOptions = configuration.GetSection(nameof(AppOptions)).Get<AppOptions>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddScoped((serviceProvider) =>
            {
                var option = CreateContextOptions(appOptions.ReadDatabaseConnectionString);
                return new CleanArchReadOnlyDbContext(option);
            });

            services.AddScoped((serviceProvider) =>
            {
                var option = CreateContextOptions(appOptions.WriteDatabaseConnectionString);
                return new CleanArchWriteDbContext(option);
            });

            DbContextOptions<AppDbContext> CreateContextOptions(string connectionString)
            {
                var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                                     .UseSqlServer(connectionString)
                                     .Options;

                return contextOptions;
            }

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(appOptions.WriteDatabaseConnectionString));

            services.AddScoped<IAppDbContext, AppDbContext>();

            return services;
        }
    }
}
