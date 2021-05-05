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

            services.AddMediatR(typeof(KarmaController).GetTypeInfo().Assembly);

            return services;
        }

        public static IServiceCollection AddIwentysEconomyFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<BarsPointTransactionLogService>();

            return services;
        }

        public static IServiceCollection AddIwentysQuestFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<QuestService>();

            return services;
        }
    }
}