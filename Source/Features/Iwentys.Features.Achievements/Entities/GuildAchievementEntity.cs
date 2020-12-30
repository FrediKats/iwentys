using System;

namespace Iwentys.Features.Achievements.Entities
{
    public class GuildAchievementEntity
    {
        public int GuildId { get; set; }
        //public GuildEntity Guild { get; set; }
        public int AchievementId { get; set; }
        public virtual AchievementEntity Achievement { get; set; }

        public virtual DateTime GettingTime { get; set; }

        public static GuildAchievementEntity Create(int guildId, int achievementId)
        {
            return new GuildAchievementEntity { GuildId = guildId, AchievementId = achievementId, GettingTime = DateTime.UtcNow };
        }
    }
}