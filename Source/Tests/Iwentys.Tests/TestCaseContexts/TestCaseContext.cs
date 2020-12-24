using System;
using Iwentys.Common.Databases;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Guilds;
using Iwentys.Database.Tools;
using Iwentys.Features.Achievements.Domain;
using Iwentys.Features.Achievements.Services;
using Iwentys.Features.Companies.Entities;
using Iwentys.Features.Companies.Models;
using Iwentys.Features.Companies.Services;
using Iwentys.Features.Economy.Services;
using Iwentys.Features.Gamification.Services;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Guilds.Models.Guilds;
using Iwentys.Features.Guilds.Services;
using Iwentys.Features.Newsfeeds.Services;
using Iwentys.Features.Quests.Models;
using Iwentys.Features.Quests.Services;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Enums;
using Iwentys.Features.Students.Services;
using Iwentys.Integrations.GithubIntegration;
using Iwentys.Tests.Tools;

namespace Iwentys.Tests.TestCaseContexts
{
    public partial class TestCaseContext
    {
        private readonly IwentysDbContext _context;
        public readonly IUnitOfWork UnitOfWork;

        //TODO: remove?
        public readonly GuildRepository GuildRepository;

        public readonly StudentService StudentService;
        public readonly GuildService GuildService;
        public readonly GuildMemberService GuildMemberService;
        public readonly GuildTributeService GuildTributeServiceService;
        public readonly CompanyService CompanyService;
        public readonly QuestService QuestService;
        public readonly GithubIntegrationService GithubIntegrationService;
        public readonly BarsPointTransactionLogService BarsPointTransactionLogService;
        public readonly NewsfeedService NewsfeedService;
        public readonly InterestTagService InterestTagService;
        public readonly AchievementService AchievementService;

        public static TestCaseContext Case() => new TestCaseContext();

        public TestCaseContext()
        {
            _context = TestDatabaseProvider.GetDatabaseContext();
            IUnitOfWork unitOfWork = new UnitOfWork<IwentysDbContext>(_context);
            UnitOfWork = unitOfWork;
            
            GuildRepository = new GuildRepository(_context);

            var achievementProvider = new AchievementProvider(unitOfWork);
            var githubApiAccessor = new DummyGithubApiAccessor();

            StudentService = new StudentService(unitOfWork, achievementProvider);
            GithubIntegrationService = new GithubIntegrationService(githubApiAccessor, unitOfWork);
            GuildService = new GuildService(GithubIntegrationService, unitOfWork);
            GuildMemberService = new GuildMemberService(GithubIntegrationService, unitOfWork);
            GuildTributeServiceService = new GuildTributeService(unitOfWork, GithubIntegrationService);
            CompanyService = new CompanyService(unitOfWork);
            BarsPointTransactionLogService = new BarsPointTransactionLogService(unitOfWork);
            QuestService = new QuestService(achievementProvider, BarsPointTransactionLogService, unitOfWork);
            NewsfeedService = new NewsfeedService(UnitOfWork);
            InterestTagService = new InterestTagService(UnitOfWork);
            AchievementService = new AchievementService(UnitOfWork);
        }

        public TestCaseContext WithNewStudent(out AuthorizedUser user, UserType userType = UserType.Common)
        {
            int id = RandomProvider.Random.Next(999999);

            var userInfo = new StudentEntity
            {
                Id = id,
                Role = userType,
                GithubUsername = $"{Constants.GithubUsername}{id}",
                BarsPoints = 1000
            };

            UnitOfWork.GetRepository<StudentEntity>().InsertAsync(userInfo).Wait();
            UnitOfWork.CommitAsync().Wait();
            user = AuthorizedUser.DebugAuth(userInfo.Id);
            return this;
        }

        public TestCaseContext WithMentor(GuildProfileDto guild, AuthorizedUser admin, out AuthorizedUser mentor)
        {
            WithGuildMentor(guild, out mentor);
            return this;
        }

        public TestCaseContext WithCompany(out CompanyInfoDto companyInfo)
        {
            var company = new CompanyEntity();
            company = CompanyService.Create(company).Result;
            companyInfo = new CompanyInfoDto(company);
            return this;
        }

        public TestCaseContext WithCompanyWorker(CompanyInfoDto companyInfo, out AuthorizedUser userInfo)
        {
            //TODO: move save changes to repository
            WithNewStudent(out userInfo);
            WithNewStudent(out AuthorizedUser admin, UserType.Admin);
            
            CompanyService.RequestAdding(companyInfo.Id, userInfo.Id).Wait();
            CompanyService.ApproveAdding(userInfo.Id, admin.Id).Wait();
            
            return this;
        }

        public TestCaseContext WithStudentProject(AuthorizedUser userInfo, out GithubProjectEntity githubProjectEntity)
        {
            var project = new GithubProjectEntity
            {
                //TODO: hack for work with dummy github
                Id = 17,
                StudentId = userInfo.Id,
                Owner = StudentService.GetAsync(userInfo.Id).Result.GithubUsername,
                Name = "Test repo"
            };

            UnitOfWork.GetRepository<GithubProjectEntity>().InsertAsync(project).Wait();
            githubProjectEntity = project;
            UnitOfWork.CommitAsync().Wait();
            
            return this;
        }

        public TestCaseContext WithQuest(AuthorizedUser user, int price, out QuestInfoDto quest)
        {
            var request = new CreateQuestRequest(
                "Some quest",
                "Some desc",
                price,
                DateTime.UtcNow.AddDays(1));
            
            quest = QuestService.CreateAsync(user, request).Result;

            return this;
        }

        private static class Constants
        {
            public const string GithubUsername = "GhUser";
        }

        public TestCaseContext WithGithubRepository(AuthorizedUser userInfo, out GithubUserEntity userEntity)
        {
            userEntity = GithubIntegrationService.CreateOrUpdate(userInfo.Id).Result;
            return this;
        }
    }
}