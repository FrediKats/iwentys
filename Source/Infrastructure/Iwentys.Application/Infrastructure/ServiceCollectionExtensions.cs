using System.Reflection;
using Iwentys.Features.Extended.Services;
using Iwentys.Features.Gamification.Karmas;
using Iwentys.Features.Gamification.Services;
using Iwentys.Features.GithubIntegration.GithubIntegration;
using Iwentys.Features.Guilds.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.Guilds.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysGuildFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<GuildMemberService>();
            services.AddScoped<GuildService>();
            services.AddScoped<GuildTestTaskService>();

            return services;
        }

        public static IServiceCollection AddIwentysTributesFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<GuildTributeService>();

            return services;
        }

        public static IServiceCollection AddIwentysGithubIntegrationFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<GithubIntegrationService>();

            return services;
        }

        public static IServiceCollection AddIwentysGamificationFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<StudyLeaderboardService>();

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

        public static IServiceCollection AddIwentysRaidFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<RaidService>();

            return services;
        }
    }
}