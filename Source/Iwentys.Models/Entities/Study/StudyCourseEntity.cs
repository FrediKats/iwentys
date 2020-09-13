using Iwentys.Models.Types;

namespace Iwentys.Models.Entities.Study
{
    public class StudyCourseEntity
    {
        public int Id { get; set; }
        public StudentGraduationYear GraduationYear { get; set; }

        public int StudyProgramId { get; set; }
        public StudyProgramEntity StudyProgramEntity { get; set; }
    }
}
