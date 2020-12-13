namespace Persistance
{
    using Common.General;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Persistance.Db;

    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            var appOptions = configuration.GetSection(nameof(AppOptions)).Get<AppOptions>();

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(appOptions.DatabaseConnectionString));

            services.AddScoped<IAppDbContext, AppDbContext>();

            return services;
        }
    }
}
