using System.Collections.Generic;
using Iwentys.Features.Achievements.ViewModels;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Integrations.GithubIntegration.Models;

namespace Iwentys.Features.Guilds.ViewModels.Guilds
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

        public ActiveTributeResponse Tribute { get; set; }
        public List<AchievementInfoDto> Achievements { get; set; }
        public List<GithubRepository> PinnedRepositories { get; set; }
        public List<GuildTestTaskInfoResponse> TestTasks { get; set; } = new List<GuildTestTaskInfoResponse>();

        public UserMembershipState UserMembershipState { get; set; }
    }
}