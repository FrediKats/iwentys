using Iwentys.Common;

namespace Iwentys.Domain.Study
{
    public class GroupAdminUser
    {
        public GroupAdminUser(Student student, StudyGroup studyGroup)
        {
            if (student.Group is null)
                throw InnerLogicException.StudyExceptions.UserIsNotGroupAdmin(student.Id);

            if (student.Id != studyGroup.GroupAdminId)
                throw InnerLogicException.StudyExceptions.UserIsNotGroupAdmin(student.Id);

            Student = student;
        }

        public Student Student { get; }
    }

    public static class GroupAdminUserExtensions
    {
        public static GroupAdminUser EnsureIsGroupAdmin(this Student profile)
        {
            return new GroupAdminUser(profile, profile.Group);
        }
    }
}