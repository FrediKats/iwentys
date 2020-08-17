using System.Collections.Generic;
using Iwentys.Models.Transferable.Students;
using Iwentys.Models.Types.Github;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Models.Transferable.Guilds
{
    public class GuildProfileDto : GuildProfileShortInfoDto
    {
        public StudentPartialProfileDto Leader { get; set; }

        public GuildMemberLeaderBoard MemberLeaderBoard { get; set; }

        //TODO: add newsfeeds
        public ActiveTributeDto Tribute { get; set; }
        public List<AchievementInfoDto> Achievements { get; set; }
        public List<GithubRepository> PinnedRepositories { get; set; }

        public UserMembershipState UserMembershipState { get; set; }
    }
}