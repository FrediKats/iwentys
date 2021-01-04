using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Tests.Tools;

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
            int id = RandomProvider.Random.Next(999999);

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