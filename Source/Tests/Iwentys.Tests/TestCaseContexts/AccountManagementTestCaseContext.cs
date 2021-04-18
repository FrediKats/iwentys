using Iwentys.Database.Seeding.FakerEntities;
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

        public AuthorizedUser WithUser(bool isAdmin = false)
        {
            IwentysUserCreateArguments createArguments = UsersFaker.Instance.IwentysUsers.Generate();
            createArguments.IsAdmin = isAdmin;
            createArguments.Id = UsersFaker.Instance.GetIdentifier();

            IwentysUserInfoDto iwentysUserInfoDto = _context.IwentysUserService.Create(createArguments).Result;

            return AuthorizedUser.DebugAuth(iwentysUserInfoDto.Id);
        }
    }
}