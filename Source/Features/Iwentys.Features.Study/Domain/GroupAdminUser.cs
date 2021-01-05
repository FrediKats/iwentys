using Iwentys.Common.Exceptions;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Study.Domain
{
    public class GroupAdminUser
    {
        public GroupAdminUser(Student student, StudyGroup studyGroup)
        {
            if (student.Id != studyGroup.GroupAdminId)
                throw InnerLogicException.NotEnoughPermissionFor(student.Id);

            Student = student;
        }

        public Student Student { get; }
    }

    public static class GroupAdminUserExtensions
    {
        //TODO: refactor
        public static GroupAdminUser EnsureIsGroupAdmin(this Student profile)
        {
            if (profile.GroupMember?.Group is null)
                throw new InnerLogicException("Student without group");
            return new GroupAdminUser(profile, profile.GroupMember?.Group);
        }
    }
}