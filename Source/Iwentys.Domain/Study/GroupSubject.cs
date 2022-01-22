namespace Iwentys.Domain.Study;

public class GroupSubject
{
    public int Id { get; init; }

    public int SubjectId { get; init; }
    public virtual Subject Subject { get; init; }
    public StudySemester StudySemester { get; init; }

    public int StudyGroupId { get; init; }

    public GroupSubject()
    {
    }

    //TODO: enable nullability
    public GroupSubject(Subject subject, int studyGroupId, StudySemester studySemester)
    {
        Subject = subject;
        SubjectId = subject.Id;
        StudyGroupId = studyGroupId;
        StudySemester = studySemester;
    }
}