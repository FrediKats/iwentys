using System.Collections.Generic;

namespace Iwentys.Domain.Gamification
{
    public class AchievementProvider
    {
        public List<GuildAchievement> GuildAchievement { get; }
        public List<StudentAchievement> StudentAchievement { get; }

        public AchievementProvider()
        {
            GuildAchievement = new List<GuildAchievement>();
            StudentAchievement = new List<StudentAchievement>();
        }

        public void AchieveForStudent(Achievement achievement, int studentId)
        {
            StudentAchievement.Add(Gamification.StudentAchievement.Create(studentId, achievement.Id));
        }

        public void AchieveForGuild(Achievement achievement, int guildId)
        {
            GuildAchievement.Add(Gamification.GuildAchievement.Create(guildId, achievement.Id));
        }
    }
}