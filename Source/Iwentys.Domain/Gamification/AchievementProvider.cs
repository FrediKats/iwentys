using System.Collections.Generic;
using Iwentys.Domain.Achievements;

namespace Iwentys.Domain.Gamification;

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
        StudentAchievement.Add(Achievements.StudentAchievement.Create(studentId, achievement.Id));
    }

    public void AchieveForGuild(Achievement achievement, int guildId)
    {
        GuildAchievement.Add(Achievements.GuildAchievement.Create(guildId, achievement.Id));
    }
}