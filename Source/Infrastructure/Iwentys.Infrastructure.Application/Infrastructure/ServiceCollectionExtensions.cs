using Iwentys.Domain.Gamification;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Infrastructure.Application.Companies;
using Iwentys.Infrastructure.Application.GithubIntegration;
using Iwentys.Infrastructure.Application.Guilds;
using Iwentys.Infrastructure.Application.Quests;
using Iwentys.Infrastructure.Application.Services;
using Iwentys.Infrastructure.Application.StudentProfile;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Infrastructure.DataAccess.Subcontext;
using Iwentys.Integrations.GithubIntegration;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Infrastructure.Application.Infrastructure
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