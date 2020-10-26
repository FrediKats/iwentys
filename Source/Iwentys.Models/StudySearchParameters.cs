using Iwentys.Models.Types;

namespace Iwentys.Models
{
    public class StudySearchParameters
    {
        public int? GroupId { get; set; }
        public int? SubjectId { get; set; }
        public int? CourseId { get; set; }
        public StudySemester? StudySemester { get; set; }
    }
}