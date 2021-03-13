namespace Api
{
    using ApiFramework.Attributes;
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using System;

    public static class DependencyInjection
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddSwaggerOptions();
            services.AddHttpContextAccessor();

            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateAttribute));
            })
            .AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

            return services;
        }

        public static IApplicationBuilder UseWebApi(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseAppSwagger(configuration);
            app.UseStaticFiles();
            app.UseRouting();
            //app.UseAuthentication();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }

        #region Swagger
        public static IServiceCollection AddSwaggerOptions(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CleanTemplate.WebUI",
                    Description = "This is a solution template for Clean Architecture implementation with ASP.NET Core Web Api",
                    Contact = new OpenApiContact
                    {
                        Name = "Omid Ahmadpour",
                        Email = "ahmadpooromid@gmail.com",
                        Url = new Uri("https://github.com/omidah"),
                    },
                });
            });

            return services;
        }

        public static IApplicationBuilder UseAppSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanTemplate.WebUI v1"));

            return app;
        }
        #endregion

    }
}

