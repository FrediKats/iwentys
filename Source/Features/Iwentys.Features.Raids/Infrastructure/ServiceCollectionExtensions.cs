using Iwentys.Features.Raids.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.Raids.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysRaidFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<RaidService>();

            return services;
        }
    }
}