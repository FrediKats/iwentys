using System;
using Iwentys.Common.Tools;
using Iwentys.Core.Services;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Achievements;
using Iwentys.Database.Repositories.Guilds;
using Iwentys.Database.Repositories.Study;
using Iwentys.Features.Achievements;
using Iwentys.Features.Companies.Entities;
using Iwentys.Features.Companies.Enums;
using Iwentys.Features.Companies.Services;
using Iwentys.Features.Companies.ViewModels;
using Iwentys.Features.Economy.Services;
using Iwentys.Features.GithubIntegration;
using Iwentys.Features.Guilds;
using Iwentys.Features.Guilds.Services;
using Iwentys.Features.StudentFeature;
using Iwentys.Features.StudentFeature.Services;
using Iwentys.Integrations.GithubIntegration;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Github;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Transferable;
using Iwentys.Models.Transferable.Gamification;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.GuildTribute;
using Iwentys.Models.Types;

namespace Iwentys.Tests.Tools
{
    public class TestCaseContext
    {
        public readonly IwentysDbContext Context;

        public readonly StudentRepository StudentRepository;
        public readonly GuildRepository GuildRepository;

        public readonly DatabaseAccessor DatabaseAccessor;

        public readonly StudentService StudentService;
        public readonly GuildService GuildService;
        public readonly GuildMemberService GuildMemberService;
        public readonly GuildTributeService GuildTributeServiceService;
        public readonly CompanyService CompanyService;
        public readonly QuestService QuestService;
        public readonly GithubUserDataService GithubUserDataService;

        public static TestCaseContext Case() => new TestCaseContext();

        public TestCaseContext()
        {
            Context = TestDatabaseProvider.GetDatabaseContext();
            StudentRepository = new StudentRepository(Context);
            GuildRepository = new GuildRepository(Context);

            DatabaseAccessor = new DatabaseAccessor(Context);
            var achievementProvider = new AchievementProvider(new AchievementRepository(Context));
            DummyGithubApiAccessor githubApiAccessor = new DummyGithubApiAccessor();

            GuildRepositoriesScope database = new GuildRepositoriesScope(DatabaseAccessor.Student, DatabaseAccessor.Guild, DatabaseAccessor.GuildMember, DatabaseAccessor.GuildTribute);

            StudentService = new StudentService(DatabaseAccessor.Student, achievementProvider);
            GithubUserDataService = new GithubUserDataService(githubApiAccessor, DatabaseAccessor.GithubUserData, DatabaseAccessor.StudentProject, DatabaseAccessor.Student);
            GuildService = new GuildService(database, GithubUserDataService, githubApiAccessor);
            GuildMemberService = new GuildMemberService(GithubUserDataService, githubApiAccessor, database.Student, database.Guild, database.GuildMember, database.GuildTribute);
            GuildTributeServiceService = new GuildTributeService(database, githubApiAccessor, DatabaseAccessor.StudentProject);
            CompanyService = new CompanyService(DatabaseAccessor.Company, DatabaseAccessor.Student);
            QuestService = new QuestService(DatabaseAccessor.Student, DatabaseAccessor.Quest, achievementProvider);
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

            StudentEntity student = StudentRepository.CreateAsync(userInfo).Result;
            user = AuthorizedUser.DebugAuth(student.Id);
            return this;
        }

        public TestCaseContext WithGuild(AuthorizedUser user, out GuildProfileDto guildProfile)
        {
            GuildProfileShortInfoDto guild = GuildService.CreateAsync(user, new GuildCreateRequest()).Result;
            guildProfile = guild.To(g => GuildService.GetAsync(g.Id, user.Id).Result);
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
            //TODO: move save changes to repository
            Context.SaveChanges();
            return this;
        }

        public TestCaseContext WithMentor(GuildProfileDto guild, AuthorizedUser admin, out AuthorizedUser mentor)
        {
            WithGuildMentor(guild, out mentor);
            return this;
        }

        public TestCaseContext WithCompany(out CompanyViewModel companyInfo)
        {
            var company = new CompanyEntity();
            company = DatabaseAccessor.Company.CreateAsync(company).Result;
            companyInfo = CompanyViewModel.Create(company);
            return this;
        }

        public TestCaseContext WithCompanyWorker(CompanyViewModel companyInfo, out AuthorizedUser userInfo)
        {
            //TODO: move save changes to repository
            WithNewStudent(out userInfo);
            Context.CompanyWorkers.Add(new CompanyWorkerEntity {CompanyId = companyInfo.Id, WorkerId = userInfo.Id, Type = CompanyWorkerType.Accepted});
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
                Author = userInfo.GetProfile(DatabaseAccessor.Student).Result.GithubUsername,
                Name = "Test repo"
            };
            githubProjectEntity = DatabaseAccessor.StudentProject.Create(project);

            return this;
        }

        public TestCaseContext WithTribute(AuthorizedUser userInfo, CreateProjectRequest project, out TributeInfoResponse tribute)
        {
            tribute = GuildTributeServiceService.CreateTribute(userInfo, project).Result;
            return this;
        }

        public TestCaseContext WithTribute(AuthorizedUser userInfo, GithubProjectEntity projectEntity, out TributeInfoResponse tribute)
        {
            tribute = GuildTributeServiceService.CreateTribute(userInfo, new CreateProjectRequest
            {
                Owner = userInfo.GetProfile(DatabaseAccessor.Student).Result.GithubUsername,
                RepositoryName = projectEntity.Name
            }).Result;
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
            quest = QuestService.CreateAsync(user, new CreateQuestRequest
            {
                Title = "Some quest",
                Description = "Some desc",
                Deadline = DateTime.UtcNow.AddDays(1),
                Price = price
            }).Result;

            return this;
        }

        private static class Constants
        {
            public const string GithubUsername = "GhUser";
        }

        public TestCaseContext WithGithubRepository(AuthorizedUser userInfo, out GithubUserEntity userEntity)
        {
            userEntity = GithubUserDataService.CreateOrUpdate(userInfo.Id).Result;
            return this;
        }
    }
}