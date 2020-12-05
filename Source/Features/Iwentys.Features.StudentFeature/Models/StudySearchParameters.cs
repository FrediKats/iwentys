using Iwentys.Features.StudentFeature.Enums;

namespace Iwentys.Features.StudentFeature.Models
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