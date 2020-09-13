namespace Iwentys.Models.Entities.Study
{
    public class SubjectActivityEntity
    {
        public int SubjectForGroupId { get; set; }
        public GroupSubjectEntity GroupSubjectEntity { get; set; }

        public int StudentId { get; set; }
        public StudentEntity Student { get; set; }

        public double Points { get; set; }
    }
}