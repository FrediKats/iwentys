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
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Types;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Tests.Tools
{
    public class TestCaseContext
    {
        public readonly IwentysDbContext Context;

        public readonly IStudentRepository StudentRepository;
        public readonly IGuildRepository GuildRepository;
        public readonly ICompanyRepository CompanyRepository;
        public readonly IStudentProjectRepository StudentProjectRepository;
        public readonly ITributeRepository TributeRepository;

        public readonly IStudentService StudentService;
        public readonly IGuildService GuildService;
        public readonly CompanyService CompanyService;

        public static TestCaseContext Case() => new TestCaseContext();

        public TestCaseContext()
        {
            Context = TestDatabaseProvider.GetDatabaseContext();
            StudentRepository = new StudentRepository(Context);
            GuildRepository = new GuildRepository(Context);
            CompanyRepository = new CompanyRepository(Context);
            StudentProjectRepository = new StudentProjectRepository(Context);
            TributeRepository = new TributeRepository(Context);

            var accessor = new DatabaseAccessor(Context);

            StudentService = new StudentService(StudentRepository, new DebugIsuAccessor(), new AchievementProvider(accessor));
            GuildService = new GuildService(GuildRepository, StudentRepository, StudentProjectRepository, TributeRepository, accessor, new DummyGithubApiAccessor());
            CompanyService = new CompanyService(CompanyRepository, StudentRepository);
        }

        public TestCaseContext WithNewStudent(out AuthorizedUser user, UserType userType = UserType.Common)
        {
            var userInfo = new Student
            {
                Id = RandomProvider.Random.Next(999999),
                Role = userType
            };

            user = AuthorizedUser.DebugAuth(StudentRepository.Create(userInfo));
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
            Context.GuildMembers.Add(GuildMember.NewMember(guild.Id, user.Id));
            Context.SaveChanges();
            return this;
        }

        public TestCaseContext WithGuildMentor(GuildProfileDto guild, out AuthorizedUser user)
        {
            WithNewStudent(out user);
            Context.GuildMembers.Add(new GuildMember() {GuildId = guild.Id, MemberId = user.Id, MemberType = GuildMemberType.Mentor});
            Context.SaveChanges();
            return this;
        }

        public TestCaseContext WithGuildRequest(GuildProfileDto guild, out AuthorizedUser user)
        {
            WithNewStudent(out user);
            Context.GuildMembers.Add(new GuildMember() {GuildId = guild.Id, MemberId = user.Id, MemberType = GuildMemberType.Requested});
            Context.SaveChanges();
            return this;
        }

        public TestCaseContext WithGuildBlocked(GuildProfileDto guild, out AuthorizedUser user)
        {
            WithNewStudent(out user);
            Context.GuildMembers.Add(new GuildMember() {GuildId = guild.Id, MemberId = user.Id, MemberType = GuildMemberType.Blocked});
            Context.SaveChanges();
            return this;
        }

        public TestCaseContext WithTotem(GuildProfileDto guild, AuthorizedUser admin, out AuthorizedUser totem)
        {
            WithGuildMember(guild, out totem);
            GuildService.SetTotem(admin, guild.Id, totem.Id);
            return this;
        }

        public TestCaseContext WithCompany(out CompanyInfoDto companyInfo)
        {
            var company = new Company();
            company = CompanyRepository.Create(company);
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

        public TestCaseContext WithStudentProject(AuthorizedUser userInfo, out StudentProject studentProject)
        {
            var project = new StudentProject
            {
                StudentId = userInfo.Id
            };
            studentProject = StudentProjectRepository.Create(project);

            return this;
        }

        public TestCaseContext WithTribute(AuthorizedUser userInfo, StudentProject project, out Tribute tribute)
        {
            tribute = GuildService.CreateTribute(userInfo, project.Id);
            return this;
        }
    }
}