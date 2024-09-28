using GHSTShipping.Application.Interfaces;
using GHSTShipping.Infrastructure.Resources.Services;
using GHSTShipping.Infrastructure.Resources.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace GHSTShipping.Infrastructure.Resources
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddResourcesInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ITranslator, Translator>();

            // Register EmailSender with constructor injection from appsettings
            services.AddTransient<IEmailSender>(provider =>
            {
                var smtpSettings = provider.GetRequiredService<IOptions<SmtpSettings>>().Value;
                return new EmailSender(smtpSettings.ClientHost, smtpSettings.Server, smtpSettings.Port, smtpSettings.User, smtpSettings.Password);
            });

            services.Configure<SmtpSettings>(configuration.GetSection(nameof(SmtpSettings)));

            return services;
        }
    }
}
