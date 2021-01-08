using Iwentys.Common.Databases;
using Iwentys.Database.Context;
using Iwentys.Database.Tools;
using Iwentys.Features.AccountManagement.Services;
using Iwentys.Features.Achievements.Domain;
using Iwentys.Features.Achievements.Services;
using Iwentys.Features.Assignments.Services;
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
using Iwentys.Features.Raids.Services;
using Iwentys.Features.Study.Services;
using Iwentys.Features.Study.SubjectAssignments.Services;
using Iwentys.Integrations.GithubIntegration;
using Iwentys.Tests.Tools;

namespace Iwentys.Tests.TestCaseContexts
{
    public class TestCaseContext
    {
        private readonly IwentysDbContext _context;
        public readonly AccountManagementTestCaseContext AccountManagementTestCaseContext;
        public readonly AchievementService AchievementService;
        public readonly AssignmentService AssignmentService;
        public readonly AssignmentTestCaseContext AssignmentTestCaseContext;
        public readonly BarsPointTransactionLogService BarsPointTransactionLogService;
        public readonly CompanyService CompanyService;
        public readonly CompanyTestCaseContext CompanyTestCaseContext;
        public readonly GamificationTestCaseContext GamificationTestCaseContext;
        public readonly GithubIntegrationService GithubIntegrationService;
        public readonly GithubTestCaseContext GithubTestCaseContext;
        public readonly GuildMemberService GuildMemberService;
        public readonly GuildService GuildService;
        public readonly GuildTestCaseContext GuildTestCaseContext;
        public readonly GuildTestTaskService GuildTestTaskService;
        public readonly GuildTributeService GuildTributeServiceService;
        public readonly InterestTagService InterestTagService;

        public readonly IwentysUserService IwentysUserService;
        public readonly KarmaService KarmaService;
        public readonly NewsfeedService NewsfeedService;
        public readonly NewsfeedTestCaseContext NewsfeedTestCaseContext;
        public readonly PeerReviewTestCaseContext PeerReviewTestCaseContext;
        public readonly ProjectReviewService ProjectReviewService;
        public readonly QuestService QuestService;
        public readonly QuestTestCaseContext QuestTestCaseContext;
        public readonly RaidService RaidService;
        public readonly StudentService StudentService;
        public readonly StudyService StudyService;
        public readonly StudyTestCaseContext StudyTestCaseContext;
        public readonly SubjectAssignmentService SubjectAssignmentService;
        public readonly TournamentService TournamentService;

        public readonly TributeTestCaseContext TributeTestCaseContext;
        public readonly IUnitOfWork UnitOfWork;

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
            GuildMemberService = new GuildMemberService(GithubIntegrationService, UnitOfWork, GuildService);
            GuildTributeServiceService = new GuildTributeService(UnitOfWork, GithubIntegrationService);
            TournamentService = new TournamentService(GithubIntegrationService, UnitOfWork, achievementProvider);
            CompanyService = new CompanyService(UnitOfWork);
            BarsPointTransactionLogService = new BarsPointTransactionLogService(UnitOfWork);
            QuestService = new QuestService(achievementProvider, BarsPointTransactionLogService, UnitOfWork);
            NewsfeedService = new NewsfeedService(UnitOfWork);
            InterestTagService = new InterestTagService(UnitOfWork);
            AchievementService = new AchievementService(UnitOfWork);
            StudyService = new StudyService(UnitOfWork);
            GuildTestTaskService = new GuildTestTaskService(achievementProvider, UnitOfWork, GithubIntegrationService);
            KarmaService = new KarmaService(UnitOfWork);
            ProjectReviewService = new ProjectReviewService(UnitOfWork);
            SubjectAssignmentService = new SubjectAssignmentService(UnitOfWork);
            AssignmentService = new AssignmentService(UnitOfWork);
            RaidService = new RaidService(UnitOfWork);

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
            AssignmentTestCaseContext = new AssignmentTestCaseContext(this);
        }

        public static TestCaseContext Case()
        {
            return new TestCaseContext();
        }

        public static class Constants
        {
            public const string GithubUsername = "GhUser";
        }
    }
}