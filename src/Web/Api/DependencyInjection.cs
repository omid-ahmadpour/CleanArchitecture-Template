namespace CleanTemplate.Api
{
    using Filters;
    using ApiFramework.Attributes;
    using ApiFramework.Swagger;
    using Common.Behaviours;
    using Common.General;
    using Common.Utilities;
    using Persistence.Db;
    using Common;
    using Domain.Entities.Users;
    using Domain.IRepositories;
    using FluentValidation;
    using FluentValidation.AspNetCore;
    using HealthChecks.UI.Client;
    using MediatR;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using PolyCache;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Swashbuckle.AspNetCore.SwaggerUI;
    using System;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    public static class DependencyInjection
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration configuration, SiteSettings siteSettings)
        {
            services.Configure<SiteSettings>(configuration.GetSection(nameof(SiteSettings)));
            var appOptions = configuration.GetSection(nameof(AppOptions)).Get<AppOptions>();
            var distributedCacheConfig = configuration.GetSection(nameof(DistributedCacheConfig)).Get<DistributedCacheConfig>();

            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddSwaggerOptions();
            services.AddHttpContextAccessor();
            services.AddCustomIdentity(siteSettings.IdentitySettings);
            services.AddJwtAuthentication(siteSettings.JwtSettings);
            services.AddCleanArchControllers();
            services.AddAutoMapperConfiguration();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddPolyCache(configuration);

            services.AddHealthChecks()
                    .AddSqlServer(appOptions.WriteDatabaseConnectionString)
                    .AddRedis(distributedCacheConfig.ConnectionString);
            services.AddHealthChecksUI()
                    .AddInMemoryStorage();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));

            return services;
        }

        public static IApplicationBuilder UseWebApi(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });

            app.UseAppSwagger(configuration);
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecksUI();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });

            return app;
        }

        #region Swagger
        public static IServiceCollection AddSwaggerOptions(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();

                options.SwaggerDoc("v1", new OpenApiInfo
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
                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Version = "v2",
                    Title = "CleanTemplate.WebUI",
                    Description = "This is a solution template for Clean Architecture implementation with ASP.NET Core Web Api",
                    Contact = new OpenApiContact
                    {
                        Name = "Omid Ahmadpour",
                        Email = "ahmadpooromid@gmail.com",
                        Url = new Uri("https://github.com/omidah"),
                    },
                });

                #region Filters
                //Enable to use [SwaggerRequestExample] & [SwaggerResponseExample]
                //options.ExampleFilters();

                options.OperationFilter<ApplySummariesOperationFilter>();

                //Add 401 response and security requirements (Lock icon) to actions that need authorization
                options.OperationFilter<UnauthorizedResponsesOperationFilter>(true, "OAuth2");

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                 {
                     {
                           new OpenApiSecurityScheme
                             {
                                 Reference = new OpenApiReference
                                 {
                                     Type = ReferenceType.SecurityScheme,
                                     Id = "Bearer"
                                 }
                             },
                             Array.Empty<string>()
                     }
                 });

                #region Versioning

                options.OperationFilter<RemoveVersionParameters>();

                options.DocumentFilter<SetVersionInPaths>();

                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                    var versions = methodInfo.DeclaringType
                        .GetCustomAttributes<ApiVersionAttribute>(true)
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v}" == docName);
                });
                #endregion

                //If use FluentValidation then must be use this package to show validation in swagger (MicroElements.Swashbuckle.FluentValidation)
                //options.AddFluentValidationRules();
                #endregion
            });

            return services;
        }

        public static IApplicationBuilder UseAppSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger();

            //Swagger middleware for generate UI from swagger.json
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanTemplate.WebUI v1");
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "CleanTemplate.WebUI v2");

                options.DocExpansion(DocExpansion.None);
            });

            //ReDoc UI middleware. ReDoc UI is an alternative to swagger-ui
            app.UseReDoc(options =>
            {
                options.SpecUrl("/swagger/v1/swagger.json");
                //options.SpecUrl("/swagger/v2/swagger.json");

                #region Customizing
                //By default, the ReDoc UI will be exposed at "/api-docs"
                //options.RoutePrefix = "docs";
                //options.DocumentTitle = "My API Docs";

                options.EnableUntrustedSpec();
                options.ScrollYOffset(10);
                options.HideHostname();
                options.HideDownloadButton();
                options.ExpandResponses("200,201");
                options.RequiredPropsFirst();
                options.NoAutoAuth();
                options.PathInMiddlePanel();
                options.HideLoading();
                options.NativeScrollbars();
                options.DisableSearch();
                options.OnlyRequiredInSamples();
                options.SortPropsAlphabetically();
                #endregion
            });

            return app;
        }
        #endregion

        public static void AddJwtAuthentication(this IServiceCollection services, JwtSettings jwtSettings)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var secretKey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
                var encryptionKey = Encoding.UTF8.GetBytes(jwtSettings.EncryptKey);

                var validationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero, // default: 5 min
                    RequireSignedTokens = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),

                    RequireExpirationTime = true,
                    ValidateLifetime = true,

                    ValidateAudience = true, //default : false
                    ValidAudience = jwtSettings.Audience,

                    ValidateIssuer = true, //default : false
                    ValidIssuer = jwtSettings.Issuer,

                    TokenDecryptionKey = new SymmetricSecurityKey(encryptionKey)
                };

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = validationParameters;

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception != null)
                            throw new CleanArchAppException(ApiResultStatusCode.UnAuthorized, "Authentication failed.", HttpStatusCode.Unauthorized, context.Exception, null);

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = async context =>
                    {
                        var signInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<User>>();
                        var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();

                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                        if (claimsIdentity.Claims?.Any() != true)
                            context.Fail("This token has no claims.");

                        //var securityStamp = claimsIdentity.FindFirstValue(new ClaimsIdentityOptions().SecurityStampClaimType);
                        //if (!securityStamp.HasValue())
                        //    context.Fail("This token has no security stamp");

                        //Find user and token from database and perform your custom validation
                        var userId = claimsIdentity.GetUserId<int>();
                        var user = await userRepository.GetByIdAsync(context.HttpContext.RequestAborted, userId);

                        //if (user.SecurityStamp != Guid.Parse(securityStamp))
                        //    context.Fail("Token security stamp is not valid.");

                        //var validatedUser = await signInManager.ValidateSecurityStampAsync(context.Principal);
                        //if (validatedUser == null)
                        //    context.Fail("Token security stamp is not valid.");

                        if (!user.IsActive)
                            context.Fail("User is not active.");

                        await userRepository.UpdateLastLoginDateAsync(user, context.HttpContext.RequestAborted);
                    },
                    OnChallenge = context =>
                    {
                        if (context.AuthenticateFailure != null)
                            throw new CleanArchAppException(ApiResultStatusCode.UnAuthorized, "Authenticate failure.", HttpStatusCode.Unauthorized, context.AuthenticateFailure, null);
                        throw new CleanArchAppException(ApiResultStatusCode.UnAuthorized, "You are unauthorized to access this resource.", HttpStatusCode.Unauthorized);
                    }
                };
            });
        }

        public static void AddCleanArchControllers(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ValidateModelStateAttribute));
                options.Filters.Add(new ApiExceptionFilter());
            })
            .AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<Startup>();
            });

            services.AddCors();
        }

        public static void AddCustomIdentity(this IServiceCollection services, IdentitySettings settings)
        {
            services.AddIdentity<User, Role>(identityOptions =>
            {
                //Password Settings
                identityOptions.Password.RequireDigit = settings.PasswordRequireDigit;
                identityOptions.Password.RequiredLength = settings.PasswordRequiredLength;
                identityOptions.Password.RequireNonAlphanumeric = settings.PasswordRequireNonAlphanumeric; //#@!
                identityOptions.Password.RequireUppercase = settings.PasswordRequireUppercase;
                identityOptions.Password.RequireLowercase = settings.PasswordRequireLowercase;

                //UserName Settings
                identityOptions.User.RequireUniqueEmail = settings.RequireUniqueEmail;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
        }

        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
        }
    }
}

