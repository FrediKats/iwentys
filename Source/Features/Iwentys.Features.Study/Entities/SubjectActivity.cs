using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Study.Entities
{
    public class SubjectActivity
    {
        public int GroupSubjectEntityId { get; set; }
        public virtual GroupSubject GroupSubject { get; set; }

        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        public double Points { get; set; }
    }
}