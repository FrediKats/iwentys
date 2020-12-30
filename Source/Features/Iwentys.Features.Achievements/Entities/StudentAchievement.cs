using System;

namespace Iwentys.Features.Achievements.Entities
{
    public class StudentAchievement
    {
        public int StudentId { get; set; }
        //public StudentEntity Student { get; set; }
        public int AchievementId { get; set; }
        public virtual Achievement Achievement { get; set; }

        public virtual DateTime GettingTime { get; set; }

        public static StudentAchievement Create(int studentId, int achievementId)
        {
            return new StudentAchievement {StudentId = studentId, AchievementId = achievementId, GettingTime = DateTime.UtcNow};
        }
    }
}