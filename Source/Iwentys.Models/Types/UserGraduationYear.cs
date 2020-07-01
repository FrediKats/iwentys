namespace Iwentys.Models.Types
{
    public enum UserGraduationYear
    {
        Y20 = 1,
        Y21,
        Y22,
        Y23,
        Y24
    }

    public static class UserGraduationYearExtensions
    {
        public static bool IsCurrentFirstCourse(this UserGraduationYear userGraduationYear) => userGraduationYear == UserGraduationYear.Y24;
    }
}