using Iwentys.Features.Extended.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.Extended.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysNewsfeedFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<NewsfeedService>();

            return services;
        }

        public static IServiceCollection AddIwentysPeerReviewFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<ProjectReviewService>();

            return services;
        }

        public static IServiceCollection AddIwentysRaidFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<RaidService>();

            return services;
        }
    }
}