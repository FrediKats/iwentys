using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Core.Services.Implementations;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Database.Repositories.Implementations;
using Iwentys.Models.Entities;
using Iwentys.Models.Transferable.Companies;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Types;

namespace Iwentys.Tests.Tools
{
    public class TestCaseContext
    {
        private readonly IwentysDbContext _context;

        public readonly IStudentRepository StudentRepository;
        public readonly IGuildRepository GuildRepository;
        public readonly ICompanyRepository CompanyRepository;

        public readonly IStudentService StudentService;
        public readonly IGuildService GuildService;
        public readonly CompanyService CompanyService;

        public static TestCaseContext Case() => new TestCaseContext();

        public TestCaseContext()
        {
            _context = TestDatabaseProvider.GetDatabaseContext();
            StudentRepository = new StudentRepository(_context);
            GuildRepository = new GuildRepository(_context);
            CompanyRepository = new CompanyRepository(_context);

            StudentService = new StudentService(StudentRepository);
            GuildService = new GuildService(GuildRepository, StudentRepository);
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
            guildProfile = GuildService.Create(user, new GuildCreateArgumentDto());

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
            _context.CompanyWorkers.Add(new CompanyWorker {CompanyId = companyInfo.Id, WorkerId = userInfo.Id, Type = CompanyWorkerType.Accepted});
            _context.SaveChanges();
            return this;
        }
    }
}