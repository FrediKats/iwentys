using Iwentys.EntityManager.PublicTypes;

namespace Iwentys.EntityManager.WebApiDtos;

public class SubjectTeacherCreateArgs
{
    public int SubjectId { get; set; }
    public int TeacherId { get; set; }
    public TeacherType TeacherType { get; set; }
    public IReadOnlyList<int> GroupSubjectIds { get; set; }
}