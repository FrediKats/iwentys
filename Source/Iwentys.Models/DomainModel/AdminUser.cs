using Iwentys.Models.Entities;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Types;

namespace Iwentys.Models.DomainModel
{
    public class AdminUser : UserProfile
    {
        public UserProfile UserProfile { get; }

        public AdminUser(UserProfile userProfile)
        {
            UserProfile = userProfile;
        }
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