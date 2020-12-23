using Iwentys.Common.Exceptions;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Enums;

namespace Iwentys.Features.Students.Domain
{
    public class SystemAdminUser
    {
        public SystemAdminUser(StudentEntity student)
        {
            if (student.Role != UserType.Admin)
                throw InnerLogicException.NotEnoughPermissionFor(student.Id);
            
            Student = student;
        }

        public StudentEntity Student { get; }
    }

    public static class SystemAdminUserExtensions
    {
        public static SystemAdminUser EnsureIsAdmin(this StudentEntity profile)
        {
            return new SystemAdminUser(profile);
        }
    }
}