using System.Collections.Generic;
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

    public GroupSubject AddGroup(StudyGroup studyGroup, StudySemester studySemester)
    {
        var groupSubject = new GroupSubject(this, studyGroup, studySemester);
        GroupSubjects.Add(groupSubject);
        return groupSubject;
    }
}