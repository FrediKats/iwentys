using System.Threading.Tasks;
using Iwentys.Common;

namespace Iwentys.Domain.AccountManagement
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
        public static bool CheckIsAdmin(this IwentysUser profile, out SystemAdminUser user)
        {
            if (profile.IsAdmin)
            {
                user = new SystemAdminUser(profile);
                return true;
            }

            user = null;
            return false;
        }

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