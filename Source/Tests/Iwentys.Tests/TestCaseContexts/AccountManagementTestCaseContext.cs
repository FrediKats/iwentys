using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.AccountManagement.Dto;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess.Seeding.FakerEntities;

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

            var iwentysUser = IwentysUser.Create(createArguments);

            _context.UnitOfWork.GetRepository<IwentysUser>().Insert(iwentysUser);
            _context.UnitOfWork.CommitAsync().Wait();
            IwentysUserInfoDto iwentysUserInfoDto = new IwentysUserInfoDto(iwentysUser);

            return AuthorizedUser.DebugAuth(iwentysUserInfoDto.Id);
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