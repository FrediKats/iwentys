using System.Collections.Generic;

namespace Iwentys.Domain.AccountManagement;

public class SubjectMentorCreateArgs
{
    public int SubjectId { get; set; }
    public int MentorId { get; set; }
    public IReadOnlyList<int> GroupSubjectIds { get; set; }
}