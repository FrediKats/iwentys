using System.Collections.Generic;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Study.Entities
{
    public class StudyGroupEntity
    {
        public int Id { get; set; }
        public string GroupName { get; set; }

        public int StudyCourseId { get; set; }
        public StudyCourseEntity StudyCourseEntity { get; set; }

        public List<StudentEntity> Students { get; set; }
        public List<GroupSubjectEntity> GroupSubjects { get; set; }
    }
}