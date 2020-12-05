using Iwentys.Features.Students.Enums;

namespace Iwentys.Features.Study.Models
{
    public class StudySearchParameters
    {
        public int? GroupId { get; set; }
        public int? SubjectId { get; set; }
        public int? CourseId { get; set; }
        public StudySemester? StudySemester { get; set; }
        
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}