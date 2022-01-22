using Iwentys.EntityManager.Common;
using Iwentys.EntityManager.Domain.Accounts;
using Iwentys.EntityManager.PublicTypes;

namespace Iwentys.EntityManager.Domain;

public class GroupSubject
{
    public int Id { get; init; }

    public int SubjectId { get; init; }
    public virtual Subject Subject { get; init; }
    public StudySemester StudySemester { get; init; }

    public int StudyGroupId { get; init; }
    public virtual StudyGroup StudyGroup { get; init; }

    public virtual List<GroupSubjectTeacher> Teachers { get; init; }
        
    public GroupSubject()
    {
    }

    //TODO: enable nullability
    public GroupSubject(Subject subject, StudyGroup studyGroup, StudySemester studySemester, IwentysUser lecturer)
    {
        Subject = subject;
        SubjectId = subject.Id;
        StudyGroup = studyGroup;
        StudyGroupId = studyGroup.Id;
        StudySemester = studySemester;
        Teachers = new List<GroupSubjectTeacher>
        {
            new GroupSubjectTeacher
            {
                Teacher = lecturer,
                TeacherType = TeacherType.Lecturer
            }
        };
    }

    public void AddPracticeMentor(IwentysUser practiceMentor)
    {
        AddTeacher(practiceMentor, TeacherType.Practice);
    }

    public void AddTeacher(IwentysUser teacher, TeacherType teacherType)
    {
        if (!IsUserAlreadyAdded(teacher, teacherType))
        {
            throw new IwentysException("User is already practice mentor");
        }

        Teachers.Add(new GroupSubjectTeacher
        {
            GroupSubjectId = Id,
            TeacherId = teacher.Id,
            TeacherType = teacherType
        });
    }

    private bool IsUserAlreadyAdded(IwentysUser teacher, TeacherType teacherType)
        => !Teachers.Any(t => t.TeacherId == teacher.Id && t.TeacherType == teacherType);

    public bool HasMentorPermission(IwentysUser user)
    {
        return Teachers.Any(t=> t.TeacherId == user.Id);
    }
}