using Iwentys.Database.Seeding.FakerEntities;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.AccountManagement.Entities;

namespace Iwentys.Tests.TestCaseContexts
{
    public class AccountManagementTestCaseContext
    {
        private readonly TestCaseContext _context;

        public AccountManagementTestCaseContext(TestCaseContext context)
        {
            _context = context;
        }

        public AuthorizedUser WithUser(bool isAdmin = false)
        {
            var id = UniversitySystemUserFaker.Instance.GetIdentifier();
            var user = new IwentysUser
            {
                Id = id,
                GithubUsername = $"{TestCaseContext.Constants.GithubUsername}{id}",
                BarsPoints = 1000,
                IsAdmin = isAdmin
            };

            _context.UnitOfWork.GetRepository<IwentysUser>().InsertAsync(user).Wait();
            _context.UnitOfWork.CommitAsync().Wait();
            return AuthorizedUser.DebugAuth(user.Id);
        }
    }
}