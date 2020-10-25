using Iwentys.Core.Gamification;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Core.Services.Implementations;
using Iwentys.Database.Context;
using Iwentys.Integrations.GithubIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Core.Services
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

            services.AddScoped<IBarsPointTransactionLogService, BarsPointTransactionLogService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IGithubUserDataService, GithubUserDataService>();
            services.AddScoped<IGuildMemberService, GuildMemberService>();
            services.AddScoped<IGuildService, GuildService>();
            services.AddScoped<IGuildRecruitmentService, GuildRecruitmentService>();
            services.AddScoped<IGuildTestTaskService, GuildTestTaskService>();
            services.AddScoped<IGuildTributeService, GuildTributeService>();
            services.AddScoped<IQuestService, QuestService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IStudyLeaderboardService, StudyLeaderboardService>();
            services.AddScoped<ITournamentService, TournamentService>();
        }
    }
}