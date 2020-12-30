using Iwentys.Features.Students.Enums;

namespace Iwentys.Features.Study.Entities
{
    public class StudyCourse
    {
        public int Id { get; set; }
        public virtual StudentGraduationYear GraduationYear { get; set; }

        public int StudyProgramId { get; set; }
        public virtual StudyProgram StudyProgram { get; set; }
    }
}