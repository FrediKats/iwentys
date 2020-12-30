using System;

namespace Iwentys.Features.Achievements.Entities
{
    public class StudentAchievement
    {
        public virtual DateTime GettingTime { get; init; }

        public int StudentId { get; init; }
        //public StudentEntity Student { get; set; }
        public int AchievementId { get; init; }
        public virtual Achievement Achievement { get; init; }


        public static StudentAchievement Create(int studentId, int achievementId)
        {
            return new StudentAchievement {StudentId = studentId, AchievementId = achievementId, GettingTime = DateTime.UtcNow};
        }
    }
}