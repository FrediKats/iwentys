using Iwentys.Features.Extended.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.Extended.Infrastructure
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