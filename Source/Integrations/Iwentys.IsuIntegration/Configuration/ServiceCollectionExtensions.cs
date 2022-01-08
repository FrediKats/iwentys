using Iwentys.IsuIntegration.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.IsuIntegration.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIsuIntegrationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddSingleton(IsuApplicationOptions.Load(configuration));
    }
}