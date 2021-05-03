using System;

namespace Iwentys.Domain.Achievements
{
    public class StudentAchievement
    {
        public DateTime GettingTimeUtc { get; init; }

        public int StudentId { get; init; }
        //public StudentEntity Student { get; set; }

        public int AchievementId { get; init; }
        public virtual Achievement Achievement { get; init; }


        public static StudentAchievement Create(int studentId, int achievementId)
        {
            return new StudentAchievement
            {
                StudentId = studentId,
                AchievementId = achievementId,
                GettingTimeUtc = DateTime.UtcNow
            };
        }
    }
}