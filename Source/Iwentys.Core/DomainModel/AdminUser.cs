using Iwentys.Models.Entities;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Types;

namespace Iwentys.Core.DomainModel
{
    public class AdminUser
    {
        public AdminUser(Student student)
        {
            Student = student;
        }

        public Student Student { get; }
    }

    public static class AdminUserExtensions
    {
        public static AdminUser EnsureIsAdmin(this Student profile)
        {
            if (profile.Role != UserType.Admin)
                throw InnerLogicException.NotEnoughPermission(profile.Id);

            return new AdminUser(profile);
        }

        public static AdminUser EnsureIsAdmin(this AuthorizedUser user)
        {
            return EnsureIsAdmin(user.Profile);
        }
    }
}