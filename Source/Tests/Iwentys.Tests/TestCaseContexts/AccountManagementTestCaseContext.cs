using Iwentys.DataAccess.Seeding;
using Iwentys.Domain.AccountManagement;

namespace Iwentys.Tests.TestCaseContexts
{
    public class AccountManagementTestCaseContext
    {
        private readonly TestCaseContext _context;

        public AccountManagementTestCaseContext(TestCaseContext context)
        {
            _context = context;
        }

        public IwentysUser WithIwentysUser(bool isAdmin = false)
        {
            IwentysUserCreateArguments createArguments = UsersFaker.Instance.IwentysUsers.Generate();
            createArguments.IsAdmin = isAdmin;
            createArguments.Id = UsersFaker.Instance.GetIdentifier();

            var iwentysUser = IwentysUser.Create(createArguments);
            return iwentysUser;
        }
    }
}