using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Tools;
using Iwentys.Domain.Study;

namespace Iwentys.Domain.Models
{
    public record StudyLeaderboardRowDto : IResultFormat
    {
        public StudyLeaderboardRowDto(Student student, int githubActivity)
            : this(new StudentInfoDto(student), githubActivity)
        {
        }

        public StudyLeaderboardRowDto(List<SubjectActivity> activity)
            : this(new StudentInfoDto(activity.First().Student), activity.Sum(a => a.Points))
        {
        }

        public StudyLeaderboardRowDto(StudentInfoDto student, double activity)
        {
            Student = student;
            Activity = activity;
        }

        public StudyLeaderboardRowDto()
        {
        }

        public StudentInfoDto Student { get; init; }
        public double Activity { get; init; }

        public string Format()
        {
            return $"{Student.GetFullName()} - {Activity}";
        }
    }
}