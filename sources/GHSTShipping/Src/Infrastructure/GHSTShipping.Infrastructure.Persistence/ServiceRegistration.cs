using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Interfaces.Repositories;
using GHSTShipping.Infrastructure.Persistence.Contexts;
using GHSTShipping.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Linq;
using System.Reflection;

namespace GHSTShipping.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration, bool useInMemoryDatabase)
        {
            if (useInMemoryDatabase)
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase(nameof(ApplicationDbContext)));
            }
            else
            {
                var loggerFactory = LoggerFactory.Create(loggingBuilder =>
                {
                    loggingBuilder.AddSerilog(); // Use Serilog for logging
                });

                /// Add DbContext with Serilog LoggerFactory
                /// The OPENJSON function was introduced in SQL Server 2016 (13.x); while that�s quite an old version, it�s still supported, and we don�t want to break its users by relying on it. Therefore, we�ve introduced a general way for you to tell EF which SQL Server is being targeted � this will allow us to take advantage of newer features while preserving backwards compatibility for users on older versions. To do this, simply call the new UseCompatibilityLevel method when configuring your context options:
                services.AddDbContext<ApplicationDbContext>(options =>
                    options
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection"), o => o.UseCompatibilityLevel(120))
                    .EnableSensitiveDataLogging() // Optional
                    .UseLoggerFactory(loggerFactory)); // Attach Serilog for EF Core logging
            }

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.RegisterRepositories();

            return services;
        }
        private static void RegisterRepositories(this IServiceCollection services)
        {
            var interfaceType = typeof(IGenericRepository<>);
            var interfaces = Assembly.GetAssembly(interfaceType)!.GetTypes()
                .Where(p => p.GetInterface(interfaceType.Name) != null);

            var implementations = Assembly.GetAssembly(typeof(GenericRepository<>))!.GetTypes();

            foreach (var item in interfaces)
            {
                var implementation = implementations.FirstOrDefault(p => p.GetInterface(item.Name) != null);

                if (implementation is not null)
                    services.AddTransient(item, implementation);
            }
        }
    }
}
