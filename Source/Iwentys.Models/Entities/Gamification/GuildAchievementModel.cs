using System;
using Iwentys.Models.Entities.Guilds;

namespace Iwentys.Models.Entities.Gamification
{
    public class GuildAchievementModel
    {
        public int GuildId { get; set; }
        public Guild Guild { get; set; }
        public int AchievementId { get; set; }
        public AchievementModel Achievement { get; set; }

        public DateTime GettingTime { get; set; }
    }
}