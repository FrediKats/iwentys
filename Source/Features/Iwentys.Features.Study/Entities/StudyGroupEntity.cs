using System.Collections.Generic;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Study.Entities
{
    public class StudyGroupEntity
    {
        public int Id { get; set; }
        public string GroupName { get; set; }

        public int StudyCourseId { get; set; }
        public virtual StudyCourseEntity StudyCourseEntity { get; set; }

        public virtual List<StudentEntity> Students { get; set; }
        public virtual List<GroupSubjectEntity> GroupSubjects { get; set; }
    }
}