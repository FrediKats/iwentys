namespace Iwentys.Models.Types
{
    public enum StudentGraduationYear
    {
        Y20 = 1,
        Y21,
        Y22,
        Y23,
        Y24
    }

    public static class StudentGraduationYearExtensions
    {
        public static bool IsCurrentFirstCourse(this StudentGraduationYear studentGraduationYear) => studentGraduationYear == StudentGraduationYear.Y24;
    }
}