namespace Iwentys.Domain.Study;

public class GroupSubject
{
    public int Id { get; init; }

    public int SubjectId { get; init; }
    public StudySemester StudySemester { get; init; }

    public int StudyGroupId { get; init; }

    public GroupSubject()
    {
    }

    //TODO: enable nullability
    public GroupSubject(int subjectId, int studyGroupId, StudySemester studySemester)
    {
        SubjectId = subjectId;
        StudyGroupId = studyGroupId;
        StudySemester = studySemester;
    }
}