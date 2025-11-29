using Autofac;
using Autofac.Extensions.DependencyInjection;
using CleanTemplate.Api;
using CleanTemplate.ApiFramework.Configuration;
using CleanTemplate.Application;
using CleanTemplate.Common;
using CleanTemplate.Common.General;
using CleanTemplate.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var siteSettings = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();

builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .UseSerilog((hostBuilderContext, loggerConfiguration) =>
    {
        loggerConfiguration.ReadFrom.Configuration(hostBuilderContext.Configuration);
    })
    .ConfigureContainer<ContainerBuilder>((context, containerBuilder) =>
    {
        containerBuilder.RegisterServices();
    });

builder.Services.AddWebApi(configuration, siteSettings);
builder.Services.AddPersistance(configuration);
builder.Services.AddCommon(configuration);
builder.Services.AddApplication(configuration);

var app = builder.Build();

var env = app.Environment;

if (env.IsDevelopment() || env.EnvironmentName == "Staging")
{
    app.UseDeveloperExceptionPage();
}

app.UseWebApi(configuration, env);

app.Run();
