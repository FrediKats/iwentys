using Iwentys.Common.Exceptions;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Enums;

namespace Iwentys.Features.Study.Domain
{
    public class GroupAdminUser
    {
        public GroupAdminUser(Student student)
        {
            if (student.GroupId is null)
                throw new InnerLogicException("Student without group");
            
            if (student.Role != StudentRole.GroupAdmin)
                throw InnerLogicException.NotEnoughPermissionFor(student.Id);

            Student = student;
        }

        public Student Student { get; }
    }

    public static class GroupAdminUserExtensions
    {
        public static GroupAdminUser EnsureIsGroupAdmin(this Student profile)
        {
            return new GroupAdminUser(profile);
        }
    }
}