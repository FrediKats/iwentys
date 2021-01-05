using Iwentys.Features.Gamification.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.Gamification
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysGamificationFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<StudyLeaderboardService>();
            services.AddScoped<KarmaService>();

            return services;
        }
    }
}