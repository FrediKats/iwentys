using Iwentys.Core.Gamification;
using Iwentys.Core.Services;
using Iwentys.Database.Context;
using Iwentys.Integrations.GithubIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Core
{
    public static class ServiceDiManager
    {
        public static void RegisterAbstractionsImplementation(IServiceCollection services, string githubToken)
        {
            services.AddDbContext<IwentysDbContext>(o => o.UseSqlite("Data Source=Iwentys.db"));


            if (githubToken is null)
                services.AddScoped<IGithubApiAccessor, DummyGithubApiAccessor>();
            else
            {
                GithubApiAccessor.Token = githubToken;
                services.AddScoped<IGithubApiAccessor, GithubApiAccessor>();
            }

            services.AddScoped<DatabaseAccessor>();
            services.AddScoped<AchievementProvider>();

            services.AddScoped<BarsPointTransactionLogService>();
            services.AddScoped<CompanyService>();
            services.AddScoped<GithubUserDataService>();
            services.AddScoped<GuildMemberService>();
            services.AddScoped<GuildService>();
            services.AddScoped<GuildRecruitmentService>();
            services.AddScoped<GuildTestTaskService>();
            services.AddScoped<GuildTributeService>();
            services.AddScoped<QuestService>();
            services.AddScoped<StudentService>();
            services.AddScoped<StudyLeaderboardService>();
            services.AddScoped<TournamentService>();
        }
    }
}