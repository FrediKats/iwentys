namespace Iwentys.Features.Study.Entities
{
    public class StudyGroupMember
    {
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        public int GroupId { get; set; }
        public virtual StudyGroup Group { get; set; }
    }
}