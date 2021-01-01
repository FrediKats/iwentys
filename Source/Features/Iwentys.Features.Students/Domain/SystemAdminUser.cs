using Iwentys.Common.Exceptions;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Enums;

namespace Iwentys.Features.Students.Domain
{
    public class SystemAdminUser
    {
        public SystemAdminUser(Student student)
        {
            if (student.Role != StudentRole.Admin)
                throw InnerLogicException.NotEnoughPermissionFor(student.Id);
            
            Student = student;
        }

        public Student Student { get; }
    }

    public static class SystemAdminUserExtensions
    {
        public static SystemAdminUser EnsureIsAdmin(this Student profile)
        {
            return new SystemAdminUser(profile);
        }
    }
}