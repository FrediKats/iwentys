namespace Iwentys.Models.Entities.Study
{
    public class StudyGroup
    {
        public int Id { get; set; }
        public string GroupName { get; set; }

        public int StudyStreamId { get; set; }
        public StudyStream StudyStream { get; set; }
    }
}