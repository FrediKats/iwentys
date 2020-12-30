using System.Collections.Generic;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Study.Entities
{
    public class StudyGroup
    {
        public int Id { get; set; }
        public string GroupName { get; init; }

        public int StudyCourseId { get; init; }
        public virtual StudyCourse StudyCourse { get; init; }

        public virtual List<Student> Students { get; init; }
        public virtual List<GroupSubject> GroupSubjects { get; init; }
    }
}