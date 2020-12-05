using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Tools;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Models;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Gamification.Models
{
    public class StudyLeaderboardRow : IResultFormat
    {
        public StudyLeaderboardRow()
        {
        }

        public StudyLeaderboardRow(IEnumerable<SubjectActivityEntity> activity)
        {
            List<SubjectActivityEntity> subjectActivityEntities = activity.ToList();
            Student = subjectActivityEntities[0].Student.To(s => new StudentInfoDto(s));
            Activity = subjectActivityEntities.Sum(a => a.Points);
        }

        public StudyLeaderboardRow(StudentEntity student, int githubActivity)
        {
            Student = student.To(s => new StudentInfoDto(s));
            Activity = githubActivity;
        }

        public StudentInfoDto Student { get; set; }
        public double Activity { get; set; }

        public string Format()
        {
            return $"{Student.GetFullName()} - {Activity}";
        }
    }
}