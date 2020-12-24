using Iwentys.Features.GithubIntegration.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.GithubIntegration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysGithubIntegrationFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<GithubIntegrationService>();

            return services;
        }
    }
}