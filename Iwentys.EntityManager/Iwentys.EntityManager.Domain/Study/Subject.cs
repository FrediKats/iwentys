using System.Linq.Expressions;
using Iwentys.EntityManager.PublicTypes;

namespace Iwentys.EntityManager.Domain;

public class Subject
{
    public int Id { get; init; }
    public string Title { get; init; }

    public virtual ICollection<GroupSubject> GroupSubjects { get; set; }

    public Subject()
    {
        GroupSubjects = new List<GroupSubject>();
    }

    public GroupSubject AddGroup(StudyGroup studyGroup, StudySemester studySemester, IwentysUser lecturer = null, IwentysUser practice = null)
    {
        var groupSubject = new GroupSubject(this, studyGroup, studySemester, lecturer);
        groupSubject.AddPracticeTeacher(practice);
        GroupSubjects.Add(groupSubject);
        return groupSubject;
    }

    public static Expression<Func<Subject, bool>> IsAllowedFor(int userId)
    {
        return s => s.GroupSubjects.Any(gs => gs.Teachers.Any(pm=>pm.TeacherId == userId));
    }
}