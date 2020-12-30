using System.Collections.Generic;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Study.Entities
{
    public class StudyGroup
    {
        public int Id { get; set; }
        public string GroupName { get; set; }

        public int StudyCourseId { get; set; }
        public virtual StudyCourse StudyCourse { get; set; }

        public virtual List<Student> Students { get; set; }
        public virtual List<GroupSubject> GroupSubjects { get; set; }
    }
}