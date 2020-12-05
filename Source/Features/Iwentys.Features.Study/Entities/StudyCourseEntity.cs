using Iwentys.Features.Students.Enums;

namespace Iwentys.Features.Study.Entities
{
    public class StudyCourseEntity
    {
        public int Id { get; set; }
        public StudentGraduationYear GraduationYear { get; set; }

        public int StudyProgramId { get; set; }
        public StudyProgramEntity StudyProgramEntity { get; set; }
    }
}