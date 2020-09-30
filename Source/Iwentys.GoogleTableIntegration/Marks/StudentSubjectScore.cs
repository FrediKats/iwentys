namespace Iwentys.GoogleTableIntegration.Marks
{
    public class StudentSubjectScore
    {
        public string Name { get; }
        public string Score { get; }

        public StudentSubjectScore(string name, string score)
        {
            Name = name;
            Score = score;
        }
    }
}
