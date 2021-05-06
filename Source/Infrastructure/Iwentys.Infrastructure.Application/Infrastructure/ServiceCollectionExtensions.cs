using Iwentys.Database;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Features.Extended.Companies;
using Iwentys.Features.Extended.Services;
using Iwentys.Features.Gamification.Quests;
using Iwentys.Features.Gamification.Services;
using Iwentys.Features.GithubIntegration.GithubIntegration;
using Iwentys.Features.Guilds.Guilds;
using Iwentys.Features.Guilds.Services;
using Iwentys.Features.Study.Infrastructure;
using Iwentys.Features.Study.StudentProfile;
using Iwentys.Integrations.GithubIntegration;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.Guilds.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysServices(this IServiceCollection services)
        {
            //FYI: replace after release
            services.AddScoped<IGithubApiAccessor, DummyGithubApiAccessor>();
            //services.AddScoped<IGithubApiAccessor, GithubApiAccessor>();
            services.AddScoped<AchievementProvider>();

            services.AddScoped<BarsPointTransactionLogService>();
            services.AddScoped<StudyLeaderboardService>();
            services.AddScoped<GithubIntegrationService>();

            services.AddScoped<GuildMemberService>();
            services.AddScoped<GuildService>();
            services.AddScoped<GuildTestTaskService>();

            services.AddScoped<GuildTributeService>();
            services.AddScoped<QuestService>();

            services.AddScoped<IStudyDbContext, IwentysDbContext>();

            services.AddScoped<RaidService>();

            return services;
        }

        public static IServiceCollection AddIwentysMediatorHandlers(this IServiceCollection services)
        {
            services.AddMediatR(typeof(CompanyController).Assembly);
            services.AddMediatR(typeof(QuestController).Assembly);
            services.AddMediatR(typeof(GuildController).Assembly);
            services.AddMediatR(typeof(StudentController).Assembly);

            return services;
        }
    }
}