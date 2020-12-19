using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Study.Entities
{
    public class SubjectActivityEntity
    {
        public int GroupSubjectEntityId { get; set; }
        public virtual GroupSubjectEntity GroupSubject { get; set; }

        public int StudentId { get; set; }
        public virtual StudentEntity Student { get; set; }

        public double Points { get; set; }
    }
}