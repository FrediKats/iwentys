using System;

namespace Iwentys.Domain.Achievements;

public class GuildAchievement
{
    public DateTime GettingTimeUtc { get; init; }

    public int GuildId { get; init; }
    //public GuildEntity Guild { get; set; }

    public int AchievementId { get; init; }
    public virtual Achievement Achievement { get; init; }

    public static GuildAchievement Create(int guildId, int achievementId)
    {
        return new GuildAchievement
        {
            GuildId = guildId,
            AchievementId = achievementId,
            GettingTimeUtc = DateTime.UtcNow
        };
    }
}