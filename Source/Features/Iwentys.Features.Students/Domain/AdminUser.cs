using Iwentys.Common.Exceptions;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Enums;

namespace Iwentys.Features.Students.Domain
{
    public class AdminUser
    {
        public static AdminUser EnsureIsAdmin(StudentEntity profile)
        {
            if (profile.Role != UserType.Admin)
                throw InnerLogicException.NotEnoughPermission(profile.Id);

            return new AdminUser(profile);
        }
        
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
            return AdminUser.EnsureIsAdmin(profile);
        }
    }
}