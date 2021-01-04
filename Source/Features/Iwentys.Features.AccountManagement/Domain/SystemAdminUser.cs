using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Features.AccountManagement.Entities;

namespace Iwentys.Features.AccountManagement.Domain
{
    public class SystemAdminUser
    {
        public SystemAdminUser(IwentysUser user)
        {
            if (!user.IsAdmin)
                throw InnerLogicException.NotEnoughPermissionFor(user.Id);

            User = user;
        }

        public IwentysUser User { get; }
    }

    public static class SystemAdminUserExtensions
    {
        public static SystemAdminUser EnsureIsAdmin(this IwentysUser profile)
        {
            return new SystemAdminUser(profile);
        }

        public static async Task<SystemAdminUser> EnsureIsAdmin(this Task<IwentysUser> profile)
        {
            return new SystemAdminUser(await profile);
        }
    }
}