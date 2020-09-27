using System.Collections.Generic;
using Iwentys.Models.Entities.Github;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Models.Transferable.Guilds
{
    public class GuildProfileDto : GuildProfilePreviewDto
    {
        public GuildMemberLeaderBoard MemberLeaderBoard { get; set; }

        //TODO: add newsfeeds
        public ActiveTributeDto Tribute { get; set; }
        public List<AchievementInfoDto> Achievements { get; set; }
        public List<GithubRepository> PinnedRepositories { get; set; }
        public List<GuildTestTaskInfoDto> TestTasks { get; set; } = new List<GuildTestTaskInfoDto>();

        public UserMembershipState UserMembershipState { get; set; }

        public GuildProfileDto()
        {
        }

        public GuildProfileDto(GuildEntity guild) : base(guild)
        {
        }
    }
}