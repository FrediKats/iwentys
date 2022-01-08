namespace Iwentys.Domain.Study
{
    public class StudyCourse
    {
        public int Id { get; init; }
        public virtual StudentGraduationYear GraduationYear { get; init; }

        public int StudyProgramId { get; init; }
        public virtual StudyProgram StudyProgram { get; init; }
    }
}