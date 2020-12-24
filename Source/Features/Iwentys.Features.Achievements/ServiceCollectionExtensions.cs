using Iwentys.Features.Achievements.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.Achievements
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysAchievementFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<AchievementService>();

            return services;
        }
    }
}