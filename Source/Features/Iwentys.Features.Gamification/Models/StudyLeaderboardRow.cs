using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Tools;
using Iwentys.Features.StudentFeature.Entities;
using Iwentys.Features.StudentFeature.Models;

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
            Student = subjectActivityEntities[0].Student.To(s => new StudentPartialProfileDto(s));
            Activity = subjectActivityEntities.Sum(a => a.Points);
        }

        public StudyLeaderboardRow(StudentEntity student, int githubActivity)
        {
            Student = student.To(s => new StudentPartialProfileDto(s));
            Activity = githubActivity;
        }

        public StudentPartialProfileDto Student { get; set; }
        public double Activity { get; set; }

        public string Format()
        {
            return $"{Student.GetFullName()} - {Activity}";
        }
    }
}