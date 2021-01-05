using Iwentys.Common.Databases;
using Iwentys.Database.Context;
using Iwentys.Database.Tools;
using Iwentys.Features.AccountManagement.Services;
using Iwentys.Features.Achievements.Domain;
using Iwentys.Features.Achievements.Services;
using Iwentys.Features.Companies.Services;
using Iwentys.Features.Economy.Services;
using Iwentys.Features.Gamification.Services;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Guilds.Services;
using Iwentys.Features.Guilds.Tournaments.Services;
using Iwentys.Features.Guilds.Tributes.Services;
using Iwentys.Features.InterestTags.Services;
using Iwentys.Features.Newsfeeds.Services;
using Iwentys.Features.PeerReview.Services;
using Iwentys.Features.Quests.Services;
using Iwentys.Features.Study.Services;
using Iwentys.Integrations.GithubIntegration;
using Iwentys.Tests.Tools;

namespace Iwentys.Tests.TestCaseContexts
{
    public partial class TestCaseContext
    {
        //TODO: make private
        public readonly IwentysDbContext _context;
        public readonly IUnitOfWork UnitOfWork;

        public readonly IwentysUserService IwentysUserService;
        public readonly StudentService StudentService;
        public readonly GuildService GuildService;
        public readonly GuildMemberService GuildMemberService;
        public readonly GuildTributeService GuildTributeServiceService;
        public readonly TournamentService TournamentService;
        public readonly CompanyService CompanyService;
        public readonly QuestService QuestService;
        public readonly GithubIntegrationService GithubIntegrationService;
        public readonly BarsPointTransactionLogService BarsPointTransactionLogService;
        public readonly NewsfeedService NewsfeedService;
        public readonly InterestTagService InterestTagService;
        public readonly AchievementService AchievementService;
        public readonly StudyGroupService StudyGroupService;
        public readonly GuildTestTaskService GuildTestTaskService;
        public readonly KarmaService KarmaService;
        public readonly ProjectReviewService ProjectReviewService;

        public readonly TributeTestCaseContext TributeTestCaseContext;
        public readonly GithubTestCaseContext GithubTestCaseContext;
        public readonly AccountManagementTestCaseContext AccountManagementTestCaseContext;
        public readonly StudyTestCaseContext StudyTestCaseContext;
        public readonly QuestTestCaseContext QuestTestCaseContext;
        public readonly CompanyTestCaseContext CompanyTestCaseContext;
        public readonly PeerReviewTestCaseContext PeerReviewTestCaseContext;
        public readonly GamificationTestCaseContext GamificationTestCaseContext;
        public readonly NewsfeedTestCaseContext NewsfeedTestCaseContext;
        public readonly GuildTestCaseContext GuildTestCaseContext;

        public static TestCaseContext Case() => new TestCaseContext();

        public TestCaseContext()
        {
            _context = TestDatabaseProvider.GetDatabaseContext();
            UnitOfWork = new UnitOfWork<IwentysDbContext>(_context);
            
            var achievementProvider = new AchievementProvider(UnitOfWork);
            var githubApiAccessor = new DummyGithubApiAccessor();

            IwentysUserService = new IwentysUserService(UnitOfWork);
            StudentService = new StudentService(UnitOfWork, achievementProvider);
            GithubIntegrationService = new GithubIntegrationService(githubApiAccessor, UnitOfWork);
            GuildService = new GuildService(GithubIntegrationService, UnitOfWork);
            GuildMemberService = new GuildMemberService(GithubIntegrationService, UnitOfWork);
            GuildTributeServiceService = new GuildTributeService(UnitOfWork, GithubIntegrationService);
            TournamentService = new TournamentService(GithubIntegrationService, UnitOfWork, achievementProvider);
            CompanyService = new CompanyService(UnitOfWork);
            BarsPointTransactionLogService = new BarsPointTransactionLogService(UnitOfWork);
            QuestService = new QuestService(achievementProvider, BarsPointTransactionLogService, UnitOfWork);
            NewsfeedService = new NewsfeedService(UnitOfWork);
            InterestTagService = new InterestTagService(UnitOfWork);
            AchievementService = new AchievementService(UnitOfWork);
            StudyGroupService = new StudyGroupService(UnitOfWork);
            GuildTestTaskService = new GuildTestTaskService(achievementProvider, UnitOfWork, GithubIntegrationService);
            KarmaService = new KarmaService(UnitOfWork);
            ProjectReviewService = new ProjectReviewService(UnitOfWork);

            TributeTestCaseContext = new TributeTestCaseContext(this);
            GithubTestCaseContext = new GithubTestCaseContext(this);
            AccountManagementTestCaseContext = new AccountManagementTestCaseContext(this);
            StudyTestCaseContext = new StudyTestCaseContext(this);
            QuestTestCaseContext = new QuestTestCaseContext(this);
            CompanyTestCaseContext = new CompanyTestCaseContext(this);
            PeerReviewTestCaseContext = new PeerReviewTestCaseContext(this);
            GamificationTestCaseContext = new GamificationTestCaseContext(this);
            NewsfeedTestCaseContext = new NewsfeedTestCaseContext(this);
            GuildTestCaseContext = new GuildTestCaseContext(this);
        }
        
        public static class Constants
        {
            public const string GithubUsername = "GhUser";
        }
    }
}