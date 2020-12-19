using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Tools;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Models;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Gamification.Models
{
    public record StudyLeaderboardRowDto : IResultFormat
    {
        public StudyLeaderboardRowDto(StudentEntity student, int githubActivity)
            : this(new StudentInfoDto(student), githubActivity)
        {
        }

        public StudyLeaderboardRowDto(List<SubjectActivityEntity> activity)
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