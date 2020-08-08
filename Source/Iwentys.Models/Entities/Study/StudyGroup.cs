namespace Iwentys.Models.Entities.Study
{
    public class StudyGroup
    {
        public int Id { get; set; }
        public string NamePattern { get; set; }
        public int Year { get; set; }

        public StudyProgram StudyProgram { get; set; }
        public int StudyProgramId { get; set; }

        public StudyStream StudyStream { get; set; }
        public int StudyStreamId { get; set; }
        
    }
}