using System;

namespace Iwentys.Models.Entities.Gamification
{
    public class StudentAchievementEntity
    {
        public int StudentId { get; set; }
        public StudentEntity Student { get; set; }
        public int AchievementId { get; set; }
        public AchievementEntity Achievement { get; set; }

        public DateTime GettingTime { get; set; }
    }
}