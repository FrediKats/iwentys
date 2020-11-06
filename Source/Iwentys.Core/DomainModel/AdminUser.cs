using Iwentys.Common.Exceptions;
using Iwentys.Models.Entities;
using Iwentys.Models.Types;

namespace Iwentys.Core.DomainModel
{
    public class AdminUser
    {
        public AdminUser(StudentEntity student)
        {
            Student = student;
        }

        public StudentEntity Student { get; }
    }

    public static class AdminUserExtensions
    {
        public static AdminUser EnsureIsAdmin(this StudentEntity profile)
        {
            if (profile.Role != UserType.Admin)
                throw InnerLogicException.NotEnoughPermission(profile.Id);

            return new AdminUser(profile);
        }
    }
}