namespace Iwentys.Core.GoogleTableParsing
{
    public class StudentSubjectScore
    {
        public string Group { get; }
        public string Name { get; }
        public string Score { get; }

        public StudentSubjectScore(string group, string name, string score)
        {
            Group = group;
            Name = name;
            Score = score;
        }
    }
}
