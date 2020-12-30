using System;

namespace Iwentys.Features.Achievements.Entities
{
    public class GuildAchievement
    {
        public int GuildId { get; set; }
        //public GuildEntity Guild { get; set; }
        public int AchievementId { get; set; }
        public virtual Achievement Achievement { get; set; }

        public virtual DateTime GettingTime { get; set; }

        public static GuildAchievement Create(int guildId, int achievementId)
        {
            return new GuildAchievement { GuildId = guildId, AchievementId = achievementId, GettingTime = DateTime.UtcNow };
        }
    }
}