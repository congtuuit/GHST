using GHSTShipping.Application.Interfaces;
using GHSTShipping.Infrastructure.Resources.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GHSTShipping.Infrastructure.Resources
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddResourcesInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<ITranslator, Translator>();

            return services;
        }
    }
}
