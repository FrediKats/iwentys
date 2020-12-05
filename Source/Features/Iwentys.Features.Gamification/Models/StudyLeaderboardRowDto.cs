using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Tools;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Models;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Gamification.Models
{
    public record StudyLeaderboardRowDto(StudentInfoDto Student, double Activity) : IResultFormat
    {
        public StudyLeaderboardRowDto(StudentEntity student, int githubActivity)
            : this(new StudentInfoDto(student), githubActivity)
        {
        }

        public StudyLeaderboardRowDto(List<SubjectActivityEntity> activity)
            : this(new StudentInfoDto(activity.First().Student), activity.Sum(a => a.Points))
        {
        }

        public string Format()
        {
            return $"{Student.GetFullName()} - {Activity}";
        }
    }
}