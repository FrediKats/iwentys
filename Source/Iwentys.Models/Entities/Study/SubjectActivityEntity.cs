namespace Iwentys.Models.Entities.Study
{
    public class SubjectActivityEntity
    {
        public int GroupSubjectEntityId { get; set; }
        public GroupSubjectEntity GroupSubject { get; set; }

        public int StudentId { get; set; }
        public StudentEntity Student { get; set; }

        public double Points { get; set; }
    }
}