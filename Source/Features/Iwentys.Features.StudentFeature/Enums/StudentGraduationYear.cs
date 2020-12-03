namespace Iwentys.Features.StudentFeature.Enums
{
    public enum StudentGraduationYear
    {
        Undefined = 0,
        Y20 = 1,
        Y21,
        Y22,
        Y23,
        Y24
    }

    public static class StudentGraduationYearExtensions
    {
        public static bool IsCurrentFirstCourse(this StudentGraduationYear studentGraduationYear)
        {
            return studentGraduationYear == StudentGraduationYear.Y24;
        }

        public static StudentGraduationYear Parse(string value)
        {
            if (value.StartsWith("M34"))
                return StudentGraduationYear.Y21;

            if (value.StartsWith("M33"))
                return StudentGraduationYear.Y22;

            if (value.StartsWith("M32"))
                return StudentGraduationYear.Y23;

            if (value.StartsWith("M31"))
                return StudentGraduationYear.Y24;

            if (value.StartsWith("M41"))
                return StudentGraduationYear.Y20;

            return StudentGraduationYear.Undefined;
        }
    }
}