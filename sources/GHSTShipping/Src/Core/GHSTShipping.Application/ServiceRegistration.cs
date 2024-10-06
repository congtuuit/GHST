using FluentValidation;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GHSTShipping.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly));


            services.AddAutoMapper(Assembly.GetExecutingAssembly());


            services.AddScoped<IPartnerConfigService, PartnerConfigService>();

            return services;
        }
    }
}
