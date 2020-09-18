using System;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Gamification;
using Iwentys.Core.GithubIntegration;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Core.Services.Implementations;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Database.Repositories.Implementations;
using Iwentys.IsuIntegrator;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Companies;
using Iwentys.Models.Transferable.Gamification;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.GuildTribute;
using Iwentys.Models.Types;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Tests.Tools
{
    public class TestCaseContext
    {
        public readonly IwentysDbContext Context;

        public readonly IStudentRepository StudentRepository;
        public readonly IGuildRepository GuildRepository;
        public readonly IGuildMemberRepository GuildMemberRepository;

        public readonly DatabaseAccessor DatabaseAccessor;

        public readonly IStudentService StudentService;
        public readonly IGuildService GuildService;
        public readonly IGuildMemberService GuildMemberService;
        public readonly IGuildTributeService GuildTributeServiceService;
        public readonly ICompanyService CompanyService;
        public readonly IQuestService QuestService;
        public readonly IGithubUserDataService GithubUserDataService;

        public static TestCaseContext Case() => new TestCaseContext();

        public TestCaseContext()
        {
            Context = TestDatabaseProvider.GetDatabaseContext();
            StudentRepository = new StudentRepository(Context);
            GuildRepository = new GuildRepository(Context);
            GuildMemberRepository = new GuildMemberRepository(Context);

            DatabaseAccessor = new DatabaseAccessor(Context);
            var achievementProvider = new AchievementProvider(DatabaseAccessor);
            DummyGithubApiAccessor githubApiAccessor = new DummyGithubApiAccessor();

            StudentService = new StudentService(DatabaseAccessor, new DebugIsuAccessor(), achievementProvider);
            GithubUserDataService = new GithubUserDataService(DatabaseAccessor, githubApiAccessor);
            GuildService = new GuildService(DatabaseAccessor, GithubUserDataService, githubApiAccessor);
            GuildMemberService = new GuildMemberService(DatabaseAccessor, GithubUserDataService, githubApiAccessor);
            GuildTributeServiceService = new GuildTributeService(DatabaseAccessor, githubApiAccessor);
            CompanyService = new CompanyService(DatabaseAccessor);
            QuestService = new QuestService(DatabaseAccessor, achievementProvider);
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

            user = AuthorizedUser.DebugAuth(StudentRepository.Create(userInfo).Id);
            return this;
        }

        public TestCaseContext WithGuild(AuthorizedUser user, out GuildProfileDto guildProfile)
        {
            guildProfile = GuildService.Create(user, new GuildCreateArgumentDto()).To(g => GuildService.Get(g.Id, user.Id));
            return this;
        }

        public TestCaseContext WithGuildMember(GuildProfileDto guild, out AuthorizedUser user)
        {
            WithNewStudent(out user);
            Context.GuildMembers.Add(new GuildMemberEntity(guild.Id, user.Id, GuildMemberType.Member));
            Context.SaveChanges();
            return this;
        }

        public TestCaseContext WithGuildMentor(GuildProfileDto guild, out AuthorizedUser user)
        {
            WithNewStudent(out user);
            Context.GuildMembers.Add(new GuildMemberEntity(guild.Id, user.Id, GuildMemberType.Mentor));
            Context.SaveChanges();
            return this;
        }

        public TestCaseContext WithGuildRequest(GuildProfileDto guild, out AuthorizedUser user)
        {
            WithNewStudent(out user);
            Context.GuildMembers.Add(new GuildMemberEntity(guild.Id, user.Id, GuildMemberType.Requested));
            Context.SaveChanges();
            return this;
        }

        public TestCaseContext WithGuildBlocked(GuildProfileDto guild, out AuthorizedUser user)
        {
            WithNewStudent(out user);
            Context.GuildMembers.Add(new GuildMemberEntity(guild.Id, user.Id, GuildMemberType.Blocked));
            Context.SaveChanges();
            return this;
        }

        public TestCaseContext WithMentor(GuildProfileDto guild, AuthorizedUser admin, out AuthorizedUser mentor)
        {
            WithGuildMentor(guild, out mentor);
            return this;
        }

        public TestCaseContext WithCompany(out CompanyInfoDto companyInfo)
        {
            var company = new Company();
            company = DatabaseAccessor.Company.Create(company);
            companyInfo = CompanyInfoDto.Create(company);
            return this;
        }

        public TestCaseContext WithCompanyWorker(CompanyInfoDto companyInfo, out AuthorizedUser userInfo)
        {
            WithNewStudent(out userInfo);
            Context.CompanyWorkers.Add(new CompanyWorker {CompanyId = companyInfo.Id, WorkerId = userInfo.Id, Type = CompanyWorkerType.Accepted});
            Context.SaveChanges();
            return this;
        }

        public TestCaseContext WithStudentProject(AuthorizedUser userInfo, out GithubProjectEntity githubProjectEntity)
        {
            var project = new GithubProjectEntity
            {
                //TODO: hack for work with dummy github
                Id = 17,
                StudentId = userInfo.Id,
                Author = userInfo.GetProfile(DatabaseAccessor.Student).GithubUsername,
                Name = "Test repo"
            };
            githubProjectEntity = DatabaseAccessor.StudentProject.Create(project);

            return this;
        }

        public TestCaseContext WithTribute(AuthorizedUser userInfo, CreateProjectDto project, out TributeInfoDto tribute)
        {
            tribute = GuildTributeServiceService.CreateTribute(userInfo, project);
            return this;
        }

        public TestCaseContext WithTribute(AuthorizedUser userInfo, GithubProjectEntity projectEntity, out TributeInfoDto tribute)
        {
            tribute = GuildTributeServiceService.CreateTribute(userInfo, new CreateProjectDto
            {
                Owner = userInfo.GetProfile(DatabaseAccessor.Student).GithubUsername,
                RepositoryName = projectEntity.Name
            });
            return this;
        }

        public TestCaseContext WithCompletedTribute(AuthorizedUser mentor, TributeInfoDto tribute, out TributeInfoDto completedTribute)
        {
            completedTribute = GuildTributeServiceService.CompleteTribute(mentor, new TributeCompleteDto
            {
                DifficultLevel = 1,
                Mark = 1,
                TributeId = tribute.Project.Id
            });
            return this;
        }

        public TestCaseContext WithQuest(AuthorizedUser user, int price, out QuestInfoDto quest)
        {
            quest = QuestService.Create(user, new CreateQuestDto
            {
                Title = "Some quest",
                Description = "Some desc",
                Deadline = DateTime.UtcNow.AddDays(1),
                Price = price
            });

            return this;
        }

        private static class Constants
        {
            public const string GithubUsername = "GhUser";
            public const string GithubRepoName = "GhRepo";
        }

        public TestCaseContext WithGithubRepository(AuthorizedUser userInfo, out GithubUserData userData)
        {
            userData = GithubUserDataService.CreateOrUpdate(userInfo.Id);
            return this;
        }
    }
}