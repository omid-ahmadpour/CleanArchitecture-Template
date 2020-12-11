namespace Persistance
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Persistance.Db;

    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            var appOptions = configuration.GetSection("AppOptions:DatabaseConnectionString");
            var connectionStrings = appOptions.Value;
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionStrings));

            services.AddScoped<IAppDbContext, AppDbContext>();

            return services;
        }
    }
}
