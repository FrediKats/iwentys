namespace Iwentys.GoogleTableIntegration.Marks
{
    public class StudentSubjectScore
    {
        public StudentSubjectScore(string name, string score)
        {
            Name = name;
            Score = score;
        }

        public string Name { get; }
        public string Score { get; }
    }
}