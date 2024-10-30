using Delivery.GHN;
using FluentValidation.AspNetCore;
using GHSTShipping.Application;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Infrastructure.FileManager;
using GHSTShipping.Infrastructure.FileManager.Contexts;
using GHSTShipping.Infrastructure.Identity;
using GHSTShipping.Infrastructure.Identity.Contexts;
using GHSTShipping.Infrastructure.Identity.Models;
using GHSTShipping.Infrastructure.Identity.Seeds;
using GHSTShipping.Infrastructure.Persistence;
using GHSTShipping.Infrastructure.Persistence.Contexts;
using GHSTShipping.Infrastructure.Resources;
using GHSTShipping.WebApi.Infrastructure.Extensions;
using GHSTShipping.WebApi.Infrastructure.Middlewares;
using GHSTShipping.WebApi.Infrastructure.Services;
using GHSTShipping.WebApi.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;


var builder = WebApplication.CreateBuilder(args);

// Load appsettings.{Environment}.json based on the current environment
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

bool useInMemoryDatabase = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");
bool enableSwagger = builder.Configuration.GetValue<bool>("EnableSwagger");
string env = builder.Configuration.GetValue<string>("Env");
Console.WriteLine(">>>>>>>>>>>>>>" + env + "<<<<<<<<<<<<<<<<");

builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceInfrastructure(builder.Configuration, useInMemoryDatabase);
builder.Services.AddFileManagerInfrastructure(builder.Configuration, useInMemoryDatabase);
builder.Services.AddIdentityInfrastructure(builder.Configuration, useInMemoryDatabase);
builder.Services.AddResourcesInfrastructure(builder.Configuration);
builder.Services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddSwaggerWithVersioning();
builder.Services.AddAnyCors();
builder.Services.AddCustomLocalization(builder.Configuration);
builder.Services.AddHealthChecks();

// Add GHN delivery lib
builder.Services.UseGhnApiClient();

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

#if DEBUG
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    if (!useInMemoryDatabase)
    {
        await services.GetRequiredService<IdentityContext>().Database.MigrateAsync();
        await services.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
        await services.GetRequiredService<FileManagerDbContext>().Database.MigrateAsync();
    }

    //Seed Data
    //await DefaultRoles.SeedAsync(services.GetRequiredService<RoleManager<ApplicationRole>>());
    await DefaultBasicUser.SeedAsync(services.GetRequiredService<UserManager<ApplicationUser>>());
    //await DeliveryPartnerConfig.SeedAsync(services.GetRequiredService<ApplicationDbContext>());
}
#endif


// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment() || enableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    // In production, use exception handler and HSTS
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseCustomLocalization();
app.UseAnyCors();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwaggerWithVersioning();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseHealthChecks("/health");
app.MapControllers();
app.UseSerilogRequestLogging();

app.UseDefaultFiles(); // Serve the index.html file by default
string currentDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(currentDir),
    RequestPath = ""
});

app.MapFallbackToController("Index", "Fallback"); // Fallback controller mapping

/*app.UseHttpsRedirection();
app.UseSpaStaticFiles();*/

app.UseForwardedHeaders();
app.UseMiddleware<SessionMiddleware>();


app.Run();

public partial class Program
{
}
