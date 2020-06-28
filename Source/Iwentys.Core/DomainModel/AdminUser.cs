using Iwentys.Models.Entities;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Types;

namespace Iwentys.Core.DomainModel
{
    public class AdminUser : UserProfile
    {
        public AdminUser(UserProfile userProfile)
        {
            UserProfile = userProfile;
        }

        public UserProfile UserProfile { get; }
    }

    public static class AdminUserExtensions
    {
        public static AdminUser EnsureIsAdmin(this UserProfile profile)
        {
            if (profile.Role != UserType.Admin)
                throw InnerLogicException.NotEnoughPermission(profile.Id);

            return new AdminUser(profile);
        }
    }
}