using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Tools;

namespace Iwentys.Domain.Study.Models
{
    public record StudyLeaderboardRowDto
    {
        public StudentInfoDto Student { get; init; }
        public double Activity { get; init; }

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
    }
}