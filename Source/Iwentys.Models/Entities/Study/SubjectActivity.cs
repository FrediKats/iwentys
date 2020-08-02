namespace Iwentys.Models.Entities.Study
{
    public class SubjectActivity
    {
        public int SubjectForGroupId { get; set; }
        public SubjectForGroup SubjectForGroup { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int Points { get; set; }
        
    }
}