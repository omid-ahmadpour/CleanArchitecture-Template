namespace Api
{
    using Api;
    using Api.Tools;
    using Common.General;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using System.Collections.Generic;

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

            return services;
        }

        public static IApplicationBuilder UseWebApi(this IApplicationBuilder app, IConfiguration configuration, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger(configuration, provider);
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

        public static IServiceCollection AddAppIdentityServer(this IServiceCollection services, IConfiguration configuration)
        {
            var ssoServiceUri = configuration.Get<string>(nameof(AppOptions.AuthenticationServerUri));
            var apiName = configuration.Get<string>(nameof(AppOptions.AuthenticationServerApiName));
            var apiSecret = configuration.Get<string>(nameof(AppOptions.AuthenticationServerSecret));
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                }).AddIdentityServerAuthentication(o =>
                {
                    o.Authority = ssoServiceUri;
                    o.ApiName = apiName;

                    o.RequireHttpsMetadata = false;
                    o.ApiSecret = apiSecret;
                    o.SaveToken = true;
                    o.TokenRetriever = CustomTokenRetriever.FromHeaderAndQueryString;
                });

            return services;
        }

        #region Swagger
        public static IServiceCollection AddSwaggerOptions(this IServiceCollection services)
        {
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();
            services.AddSwaggerGen(setupAction =>
            {
                setupAction.DescribeAllEnumsAsStrings();
                setupAction.DescribeAllParametersInCamelCase();
                setupAction.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "bearer"
                    }
                );

                setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                  {
                    new OpenApiSecurityScheme
                    {
                      Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                      Scheme = "oauth2",
                      Name = "Bearer",
                      In = ParameterLocation.Header
                    },
                    new List<string>()
                  }
                });
                setupAction.EnableAnnotations(true);
            });
            return services;
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, IConfiguration configuration, IApiVersionDescriptionProvider provider)
        {
            var appOptions = configuration.GetSection(nameof(AppOptions)).Get<AppOptions>();

            if (appOptions.ActivateSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(
                    options =>
                    {
                        foreach (var description in provider.ApiVersionDescriptions)
                        {
                            options.SwaggerEndpoint(
                                $"/swagger/{description.GroupName}/swagger.json",
                                description.GroupName.ToUpperInvariant());
                        }
                    });
            }

            return app;
        }
        #endregion

    }
}

