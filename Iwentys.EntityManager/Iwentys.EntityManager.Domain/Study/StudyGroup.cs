using System.Linq.Expressions;
using Iwentys.EntityManager.Common;

namespace Iwentys.EntityManager.Domain;

public class StudyGroup
{
    public int Id { get; set; }
    public string GroupName { get; init; }

    public int StudyCourseId { get; init; }
    public virtual StudyCourse StudyCourse { get; set; }

    public int? GroupAdminId { get; set; }
    //public Student GroupAdmin { get; set; }

    public virtual List<Student> Students { get; set; }
    public virtual List<GroupSubject> GroupSubjects { get; set; }

    public static Expression<Func<StudyGroup, bool>> IsMatch(GroupName groupName)
    {
        return studyGroup => studyGroup.GroupName == groupName.Name;
    }

    public StudyGroup()
    {
        Students = new List<Student>();
        GroupSubjects = new List<GroupSubject>();
    }

    public static StudyGroup MakeGroupAdmin(IwentysUser initiatorProfile, Student newGroupAdmin)
    {
        if (newGroupAdmin.Group is null)
        {
            throw new InnerLogicException($"Cannot set user {newGroupAdmin.Id} group admin. User do not have study group.");
        }

        newGroupAdmin.Group.MakeAdmin(initiatorProfile, newGroupAdmin);

        return newGroupAdmin.Group;
    }

    public void AddStudent(Student student)
    {
        Students.Add(student);
    }

    public void MakeAdmin(IwentysUser initiatorProfile, Student newGroupAdmin)
    {
        initiatorProfile.EnsureIsAdmin();
        GroupAdminId = newGroupAdmin.Id;
    }
}