using Iwentys.EntityManager.Common;

namespace Iwentys.EntityManager.Domain;

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