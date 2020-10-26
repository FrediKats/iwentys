using System.Collections.Generic;
using Iwentys.Models.Entities.Gamification;
using Iwentys.Models.Types;

namespace Iwentys.Models.Entities.Guilds
{
    public class GuildEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Bio { get; set; }
        public string LogoUrl { get; set; }
        public string TestTaskLink { get; set; }

        public GuildHiringPolicy HiringPolicy { get; set; }
        public GuildType GuildType { get; set; }

        public List<GuildMemberEntity> Members { get; set; } = new List<GuildMemberEntity>();
        public List<GuildPinnedProjectEntity> PinnedProjects { get; set; } = new List<GuildPinnedProjectEntity>();
        public List<GuildTestTaskSolvingInfoEntity> TestTasks { get; set; } = new List<GuildTestTaskSolvingInfoEntity>();

        public List<GuildAchievementEntity> Achievements { get; set; } = new List<GuildAchievementEntity>();
    }
}