using System.Collections.Generic;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Types;

namespace Iwentys.Models.Transferable.Guilds
{
    public class GuildProfileDto : GuildProfilePreviewDto
    {
        public GuildProfileDto()
        {
        }

        public GuildProfileDto(GuildEntity guild) : base(guild)
        {
        }

        public GuildMemberLeaderBoard MemberLeaderBoard { get; set; }

        //TODO: add newsfeeds
        public ActiveTributeDto Tribute { get; set; }
        public List<AchievementInfoDto> Achievements { get; set; }
        public List<GithubRepository> PinnedRepositories { get; set; }
        public List<GuildTestTaskInfoDto> TestTasks { get; set; } = new List<GuildTestTaskInfoDto>();

        public UserMembershipState UserMembershipState { get; set; }
    }
}