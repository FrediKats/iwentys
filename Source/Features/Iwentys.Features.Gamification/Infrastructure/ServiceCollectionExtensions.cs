using System.Reflection;
using Iwentys.Features.Gamification.Karmas;
using Iwentys.Features.Gamification.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.Gamification.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysGamificationFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<StudyLeaderboardService>();
            services.AddScoped<KarmaService>();

            services.AddMediatR(typeof(KarmaController).GetTypeInfo().Assembly);

            return services;
        }

        public static IServiceCollection AddIwentysInterestTagFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<InterestTagService>();

            return services;
        }
    }
}