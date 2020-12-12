using System.Collections.Generic;
using Iwentys.Features.Achievements.Entities;
using Iwentys.Features.Guilds.Enums;

namespace Iwentys.Features.Guilds.Entities
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

        public virtual List<GuildMemberEntity> Members { get; set; } = new List<GuildMemberEntity>();
        public virtual List<GuildPinnedProjectEntity> PinnedProjects { get; set; } = new List<GuildPinnedProjectEntity>();
        public virtual List<GuildTestTaskSolvingInfoEntity> TestTasks { get; set; } = new List<GuildTestTaskSolvingInfoEntity>();
        
        public virtual List<GuildAchievementEntity> Achievements { get; set; } = new List<GuildAchievementEntity>();
    }
}