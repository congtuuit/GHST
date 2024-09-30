using Microsoft.Extensions.DependencyInjection;

namespace Delivery.GHN
{
    public static class Startup
    {
        public static void UseGhnApiClient(this IServiceCollection services)
        {
            services.AddScoped<IGhnApiClient, GhnApiClient>();
        }
    }
}
