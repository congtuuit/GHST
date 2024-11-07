using FluentValidation;
using GHSTShipping.Application.Interfaces;
using GHSTShipping.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using GHSTShipping.Application.Mappers;

namespace GHSTShipping.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly));


            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddSingleton(AutoMapperConfig.Configure());


            services.AddScoped<IPartnerConfigService, PartnerConfigService>();
            services.AddScoped<IOrderCodeSequenceService, OrderCodeSequenceService>();

            

            return services;
        }
    }
}
