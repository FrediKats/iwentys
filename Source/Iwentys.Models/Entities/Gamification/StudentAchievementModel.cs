namespace Iwentys.Models.Entities.Gamification
{
    public class StudentAchievementModel
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int AchievementId { get; set; }
        public AchievementModel Achievement { get; set; }
    }
}