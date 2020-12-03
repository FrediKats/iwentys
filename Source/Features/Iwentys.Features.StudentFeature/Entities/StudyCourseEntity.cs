using Iwentys.Features.StudentFeature.Enums;

namespace Iwentys.Features.StudentFeature.Entities
{
    public class StudyCourseEntity
    {
        public int Id { get; set; }
        public StudentGraduationYear GraduationYear { get; set; }

        public int StudyProgramId { get; set; }
        public StudyProgramEntity StudyProgramEntity { get; set; }
    }
}