using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.SubjectAssignments;

namespace Iwentys.Domain.Study;

public class Subject
{
    public int Id { get; init; }
    public string Title { get; init; }

    public virtual ICollection<GroupSubject> GroupSubjects { get; set; }
    public virtual ICollection<SubjectAssignment> Assignments { get; set; }

    public Subject()
    {
        GroupSubjects = new List<GroupSubject>();
        Assignments = new List<SubjectAssignment>();
    }

    public GroupSubject AddGroup(StudyGroup studyGroup, StudySemester studySemester, IwentysUser lector = null, IwentysUser practice = null)
    {
        var groupSubject = new GroupSubject(this, studyGroup, studySemester, lector);
        groupSubject.AddPracticeMentor(practice);
        GroupSubjects.Add(groupSubject);
        return groupSubject;
    }

    public bool HasMentorPermission(IwentysUser user)
    {
        return GroupSubjects.Any(gs => gs.HasMentorPermission(user));
    }

    public static Expression<Func<Subject, bool>> IsAllowedFor(int userId)
    {
        return s => s.GroupSubjects.Any(gs => gs.Mentors.Any(pm=>pm.UserId == userId));
    }
}