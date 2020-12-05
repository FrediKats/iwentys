using System.Collections.Generic;
using Iwentys.Features.Achievements.Models;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;

namespace Iwentys.Features.Guilds.Models.Guilds
{
    public class GuildProfileDto : GuildProfilePreviewDto
    {
        public GuildProfileDto()
        {
        }

        public GuildProfileDto(GuildEntity guild) : base(guild)
        {
        }

        public GuildMemberLeaderBoardDto MemberLeaderBoardDto { get; set; }

        public ActiveTributeResponseDto Tribute { get; set; }
        public List<AchievementDto> Achievements { get; set; }
        public List<GithubRepositoryInfoDto> PinnedRepositories { get; set; }
        public List<GuildTestTaskInfoResponse> TestTasks { get; set; } = new List<GuildTestTaskInfoResponse>();

        public UserMembershipState UserMembershipState { get; set; }
    }
}