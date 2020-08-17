using System;

namespace Iwentys.Models.Entities.Gamification
{
    public class StudentAchievementEntity
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int AchievementId { get; set; }
        public AchievementModel Achievement { get; set; }

        public DateTime GettingTime { get; set; }
    }
}