namespace Iwentys.Domain.Study;

public class GroupSubject
{
    public int Id { get; init; }

    public int SubjectId { get; init; }

    public int StudyGroupId { get; init; }

    public GroupSubject()
    {
    }

    //TODO: enable nullability
    public GroupSubject(int subjectId, int studyGroupId)
    {
        SubjectId = subjectId;
        StudyGroupId = studyGroupId;
    }
}