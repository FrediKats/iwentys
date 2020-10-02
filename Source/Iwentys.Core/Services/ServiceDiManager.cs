using Iwentys.Core.Gamification;
using Iwentys.Core.GithubIntegration;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Core.Services.Implementations;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Database.Repositories.Implementations;
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
                services.AddScoped<IGithubApiAccessor, GithubApiAccessor>();

            services.AddScoped<IBarsPointTransactionLogRepository, BarsPointTransactionLogRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IGithubUserDataRepository, GithubUserDataRepository>();
            services.AddScoped<IGroupSubjectRepository, GroupGroupSubjectRepository>();
            services.AddScoped<IGuildMemberRepository, GuildMemberRepository>();
            services.AddScoped<IGuildRepository, GuildRepository>();
            services.AddScoped<IGuildTestTaskSolvingInfoRepository, GuildTestTaskSolvingInfoRepository>();
            services.AddScoped<IQuestRepository, QuestRepository>();
            services.AddScoped<IStudentProjectRepository, StudentProjectRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IStudyGroupRepository, StudyGroupRepository>();
            services.AddScoped<ISubjectActivityRepository, SubjectActivityRepository>();
            services.AddScoped<ITournamentRepository, TournamentRepository>();
            services.AddScoped<ITributeRepository, TributeRepository>();

            services.AddScoped<DatabaseAccessor>();
            services.AddScoped<AchievementProvider>();

            services.AddScoped<IBarsPointTransactionLogService, BarsPointTransactionLogService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IGithubUserDataService, GithubUserDataService>();
            services.AddScoped<IGuildMemberService, GuildMemberService>();
            services.AddScoped<IGuildService, GuildService>();
            services.AddScoped<IGuildTestTaskService, GuildTestTaskService>();
            services.AddScoped<IGuildTributeService, GuildTributeService>();
            services.AddScoped<IQuestService, QuestService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IStudyLeaderboardService, StudyLeaderboardService>();
            services.AddScoped<ITournamentService, TournamentService>();
        }
    }
}