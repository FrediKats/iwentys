using Iwentys.Integrations.IsuIntegration.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Integrations.IsuIntegration.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIsuIntegrationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddSingleton(IsuApplicationOptions.Load(configuration))
                .AddSingleton(JwtApplicationOptions.Load(configuration));
        }
    }
}
