using Iwentys.Models.Entities;

namespace Iwentys.Integrations.GoogleTableIntegration.Marks
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

        //TODO: Make check more... Just rework this
        public bool IsMatchedWithStudent(StudentEntity student)
        {
            return Name.Contains(student.FirstName)
                   && Name.Contains(student.SecondName);
        }
    }
}