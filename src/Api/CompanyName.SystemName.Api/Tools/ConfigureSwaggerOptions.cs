namespace Api.Tools
{
    using Swashbuckle.AspNetCore.SwaggerGen;

    using System;

    using Microsoft.OpenApi.Models;
    using Microsoft.Extensions.Options;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.DependencyInjection;
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) =>
            _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    new OpenApiInfo()
                    {
                        Title = $"Web Api {description.ApiVersion}",
                        Version = description.ApiVersion.ToString(),
                        Description = "All available Web apis.",
                        Contact = new OpenApiContact()
                        {
                            Email = "info@Web.ir",
                            Name = "Web",
                            Url = new Uri("https://www.WebApi.ir")
                        },
                    });
            }
        }
    }
}
