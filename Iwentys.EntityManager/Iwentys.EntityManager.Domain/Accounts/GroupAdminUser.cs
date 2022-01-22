using Iwentys.EntityManager.Common;

namespace Iwentys.EntityManager.Domain;

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