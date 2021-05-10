namespace Api
{
    using Application;
    using Common;
    using Common.General;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Persistance;

    public class Startup
    {
        private readonly SiteSettings siteSetting;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            siteSetting = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddWebApi(Configuration, siteSetting);
            services.AddPersistance(Configuration);
            services.AddCommon(Configuration);
            services.AddApplication(Configuration);

            services.AddCors();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWebApi(Configuration);
        }
    }
}