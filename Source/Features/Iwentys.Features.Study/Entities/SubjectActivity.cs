namespace Iwentys.Features.Study.Entities
{
    public class SubjectActivity
    {
        public int GroupSubjectId { get; init; }
        public virtual GroupSubject GroupSubject { get; init; }

        public int StudentId { get; init; }
        public virtual Student Student { get; init; }

        public double Points { get; set; }
    }
}