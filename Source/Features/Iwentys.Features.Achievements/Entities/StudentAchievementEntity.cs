using System;

namespace Iwentys.Features.Achievements.Entities
{
    public class StudentAchievementEntity
    {
        public int StudentId { get; set; }
        //public StudentEntity Student { get; set; }
        public int AchievementId { get; set; }
        public virtual AchievementEntity Achievement { get; set; }

        public virtual DateTime GettingTime { get; set; }
    }
}