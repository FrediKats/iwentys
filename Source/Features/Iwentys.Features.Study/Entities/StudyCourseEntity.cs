using Iwentys.Features.Students.Enums;

namespace Iwentys.Features.Study.Entities
{
    public class StudyCourseEntity
    {
        public int Id { get; set; }
        public virtual StudentGraduationYear GraduationYear { get; set; }

        public int StudyProgramId { get; set; }
        public virtual StudyProgramEntity StudyProgramEntity { get; set; }
    }
}