using System;
using Iwentys.Common.Databases;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Guilds;
using Iwentys.Database.Tools;
using Iwentys.Features.Achievements.Domain;
using Iwentys.Features.Companies.Entities;
using Iwentys.Features.Companies.Models;
using Iwentys.Features.Companies.Services;
using Iwentys.Features.Economy.Services;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Models.Guilds;
using Iwentys.Features.Guilds.Models.GuildTribute;
using Iwentys.Features.Guilds.Services;
using Iwentys.Features.Quests.Models;
using Iwentys.Features.Quests.Services;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Enums;
using Iwentys.Features.Students.Services;
using Iwentys.Integrations.GithubIntegration;

namespace Iwentys.Tests.Tools
{
    public class TestCaseContext
    {
        private readonly IwentysDbContext _context;
        public readonly IUnitOfWork UnitOfWork;

        public readonly GuildRepository GuildRepository;

        public readonly DatabaseAccessor DatabaseAccessor;

        public readonly StudentService StudentService;
        public readonly GuildService GuildService;
        public readonly GuildMemberService GuildMemberService;
        public readonly GuildTributeService GuildTributeServiceService;
        public readonly CompanyService CompanyService;
        public readonly QuestService QuestService;
        public readonly GithubIntegrationService GithubIntegrationService;
        public readonly BarsPointTransactionLogService BarsPointTransactionLogService;

        public static TestCaseContext Case() => new TestCaseContext();

        public TestCaseContext()
        {
            _context = TestDatabaseProvider.GetDatabaseContext();
            IUnitOfWork unitOfWork = new UnitOfWork<IwentysDbContext>(_context);
            UnitOfWork = unitOfWork;
            
            GuildRepository = new GuildRepository(_context);

            DatabaseAccessor = new DatabaseAccessor(_context);
            var achievementProvider = new AchievementProvider(unitOfWork);
            var githubApiAccessor = new DummyGithubApiAccessor();

            StudentService = new StudentService(unitOfWork);
            GithubIntegrationService = new GithubIntegrationService(githubApiAccessor, unitOfWork);
            GuildService = new GuildService(GithubIntegrationService, githubApiAccessor, GuildRepository, DatabaseAccessor.GuildMember, unitOfWork);
            GuildMemberService = new GuildMemberService(GithubIntegrationService, DatabaseAccessor.Guild, DatabaseAccessor.GuildMember, DatabaseAccessor.GuildTribute, unitOfWork);
            GuildTributeServiceService = new GuildTributeService(githubApiAccessor, DatabaseAccessor.Guild, DatabaseAccessor.GuildTribute, unitOfWork);
            CompanyService = new CompanyService(unitOfWork);
            BarsPointTransactionLogService =
                new BarsPointTransactionLogService(unitOfWork);
            QuestService = new QuestService(achievementProvider, BarsPointTransactionLogService, unitOfWork);
        }

        public TestCaseContext WithNewStudent(out AuthorizedUser user, UserType userType = UserType.Common)
        {
            int id = RandomProvider.Random.Next(999999);

            var userInfo = new StudentEntity
            {
                Id = id,
                Role = userType,
                GithubUsername = $"{Constants.GithubUsername}{id}"
            };

            UnitOfWork.GetRepository<StudentEntity>().InsertAsync(userInfo).Wait();
            UnitOfWork.CommitAsync().Wait();
            user = AuthorizedUser.DebugAuth(userInfo.Id);
            return this;
        }

        public TestCaseContext WithGuild(AuthorizedUser user, out ExtendedGuildProfileWithMemberDataDto guildProfile)
        {
            var guildCreateRequest = new GuildCreateRequestDto(null, null, null, GuildHiringPolicy.Close);

            GuildProfileShortInfoDto guild = GuildService.CreateAsync(user, guildCreateRequest).Result;
            guildProfile = GuildService.GetAsync(guild.Id, user.Id).Result;
            return this;
        }

        public TestCaseContext WithGuildMember(GuildProfileDto guild, AuthorizedUser guildEditor, out AuthorizedUser user)
        {
            WithNewStudent(out user);
            GuildMemberService.RequestGuildAsync(user, guild.Id).Wait();
            GuildMemberService.AcceptRequest(guildEditor, guild.Id, user.Id).Wait();
            return this;
        }

        public TestCaseContext WithGuildMentor(GuildProfileDto guild, out AuthorizedUser user)
        {
            //TODO: make method for promoting to guild editor/mentor
            WithNewStudent(out user);
            _context.GuildMembers.Add(new GuildMemberEntity(guild.Id, user.Id, GuildMemberType.Mentor));
            _context.SaveChanges();
            return this;
        }

        public TestCaseContext WithGuildRequest(GuildProfileDto guild, out AuthorizedUser user)
        {
            WithNewStudent(out user);
            GuildMemberService.RequestGuildAsync(user, guild.Id).Wait();
            return this;
        }

        public TestCaseContext WithGuildBlocked(GuildProfileDto guild, AuthorizedUser guildEditor, out AuthorizedUser user)
        {
            WithNewStudent(out user);
            GuildMemberService.RequestGuildAsync(user, guild.Id).Wait();
            GuildMemberService.BlockGuildMember(guildEditor, guild.Id, user.Id).Wait();
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

        public TestCaseContext WithTribute(AuthorizedUser userInfo, CreateProjectRequestDto project, out TributeInfoResponse tribute)
        {
            tribute = GuildTributeServiceService.CreateTribute(userInfo, project).Result;
            return this;
        }

        public TestCaseContext WithTribute(AuthorizedUser userInfo, GithubProjectEntity projectEntity, out TributeInfoResponse tribute)
        {
            var userGithub = StudentService.GetAsync(userInfo.Id).Result.GithubUsername;
            
            tribute = GuildTributeServiceService.CreateTribute(
                userInfo,
                new CreateProjectRequestDto(userGithub, projectEntity.Name))
                .Result;
            return this;
        }

        public TestCaseContext WithCompletedTribute(AuthorizedUser mentor, TributeInfoResponse tribute, out TributeInfoResponse completedTribute)
        {
            completedTribute = GuildTributeServiceService.CompleteTribute(mentor, new TributeCompleteRequest
            {
                DifficultLevel = 1,
                Mark = 1,
                TributeId = tribute.Project.Id
            }).Result;
            return this;
        }

        public TestCaseContext WithQuest(AuthorizedUser user, int price, out QuestInfoResponse quest)
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