namespace Iwentys.Models.Entities.Study
{
    public class StudyGroupEntity
    {
        public int Id { get; set; }
        public string GroupName { get; set; }

        public int StudyStreamId { get; set; }
        public StudyCourseEntity StudyCourseEntity { get; set; }
    }
}