namespace Iwentys.Domain.Study
{
    public enum StudySemester
    {
        Y19H2 = 1,
        Y20H1,
        Y20H2,
        Y21H1
    }

    public static class StudySemesterExtensions
    {
        public static StudySemester GetDefault() => StudySemester.Y21H1;
    }
}