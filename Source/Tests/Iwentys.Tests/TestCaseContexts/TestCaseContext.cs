using Iwentys.Infrastructure.Application.Controllers.GithubIntegration;
using Iwentys.Infrastructure.Application.Controllers.Services;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Tests.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Tests.TestCaseContexts
{
    public class TestCaseContext
    {
        public readonly IwentysDbContext _context;

        public readonly AccountManagementTestCaseContext AccountManagementTestCaseContext;
        public readonly GithubTestCaseContext GithubTestCaseContext;
        public readonly GuildMemberService GuildMemberService;
        public readonly GuildService GuildService;
        public readonly GuildTestCaseContext GuildTestCaseContext;

        public readonly QuestService QuestService;
        public readonly RaidService RaidService;
        public readonly StudyTestCaseContext StudyTestCaseContext;
        public readonly StudyLeaderboardService StudyLeaderboard;

        public readonly IUnitOfWork UnitOfWork;

        public TestCaseContext(ServiceProvider serviceProvider)
        {

            _context = serviceProvider.GetRequiredService<IwentysDbContext>();
            UnitOfWork = new UnitOfWork<IwentysDbContext>(_context);

            //TODO: use DI (AspStartupExtensions)
            GuildService = serviceProvider.GetRequiredService<GuildService>();
            GuildMemberService = serviceProvider.GetRequiredService<GuildMemberService>();
            QuestService = serviceProvider.GetRequiredService<QuestService>();
            RaidService = serviceProvider.GetRequiredService<RaidService>();
            StudyLeaderboard = serviceProvider.GetRequiredService<StudyLeaderboardService>();
            //IwentysUserService = new IwentysUserService(UnitOfWork);
            //StudentService = new StudentService(UnitOfWork);
            //GithubIntegrationService = new GithubIntegrationService(githubApiAccessor, UnitOfWork);
            //GuildService = new GuildService(GithubIntegrationService, UnitOfWork);
            //GuildMemberService = new GuildMemberService(GithubIntegrationService, UnitOfWork, GuildService);
            //GuildTributeServiceService = new GuildTributeService(UnitOfWork, GithubIntegrationService);
            //TournamentService = new TournamentService(GithubIntegrationService, UnitOfWork, achievementProvider);
            //CompanyService = new CompanyService(UnitOfWork);
            //BarsPointTransactionLogService = new BarsPointTransactionLogService(UnitOfWork);
            //QuestService = new QuestService(achievementProvider, BarsPointTransactionLogService, UnitOfWork);
            //NewsfeedService = new NewsfeedService(UnitOfWork);
            //InterestTagService = new InterestTagService(UnitOfWork);
            //AchievementService = new AchievementService(UnitOfWork);
            //StudyService = new StudyService(UnitOfWork);
            //KarmaService = new KarmaService(UnitOfWork);
            //ProjectReviewService = new ProjectReviewService(UnitOfWork);
            //GuildTestTaskService = new GuildTestTaskService(achievementProvider, UnitOfWork, GithubIntegrationService);
            //AssignmentService = new AssignmentService(UnitOfWork);
            //RaidService = new RaidService(UnitOfWork);
            //StudyLeaderboard = new StudyLeaderboardService(_context);

            GithubTestCaseContext = new GithubTestCaseContext(this);
            AccountManagementTestCaseContext = new AccountManagementTestCaseContext(this);
            StudyTestCaseContext = new StudyTestCaseContext(this);
            GuildTestCaseContext = new GuildTestCaseContext(this);
        }

        public static TestCaseContext Case()
        {
            ServiceProvider serviceProvider = new ServiceCollectionHolder().ServiceProvider;
            return new TestCaseContext(serviceProvider);
        }
    }
}