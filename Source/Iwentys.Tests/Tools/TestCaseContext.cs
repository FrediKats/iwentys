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

        public readonly IUserProfileRepository UserProfileRepository;
        public readonly IGuildRepository GuildRepository;
        public readonly ICompanyRepository CompanyRepository;

        public readonly IUserProfileService UserProfileService;
        public readonly IGuildService GuildService;
        public readonly CompanyService CompanyService;

        public static TestCaseContext Case() => new TestCaseContext();

        public TestCaseContext()
        {
            _context = TestDatabaseProvider.GetDatabaseContext();
            UserProfileRepository = new UserProfileRepository(_context);
            GuildRepository = new GuildRepository(_context);
            CompanyRepository = new CompanyRepository(_context);

            UserProfileService = new UserProfileService(UserProfileRepository);
            GuildService = new GuildService(GuildRepository, UserProfileRepository);
            CompanyService = new CompanyService(CompanyRepository, UserProfileRepository);
        }

        public TestCaseContext WithNewUser(out AuthorizedUser user, UserType userType = UserType.Common)
        {
            var userInfo = new UserProfile
            {
                Id = RandomProvider.Random.Next(999999),
                Role = userType
            };

            user = AuthorizedUser.DebugAuth(UserProfileRepository.Create(userInfo));
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
            WithNewUser(out userInfo);
            _context.CompanyWorkers.Add(new CompanyWorker {CompanyId = companyInfo.Id, WorkerId = userInfo.Id, Type = CompanyWorkerType.Accepted});
            _context.SaveChanges();
            return this;
        }
    }
}