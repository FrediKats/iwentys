using System.Collections.Generic;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.Guilds.Entities;

namespace Iwentys.Features.Guilds.Models
{
    public class ExtendedGuildProfileWithMemberDataDto : GuildProfileDto
    {
        public ExtendedGuildProfileWithMemberDataDto(Guild guild)
            : base(guild)
        {
        }

        public ExtendedGuildProfileWithMemberDataDto()
        {
        }

        public List<GithubRepositoryInfoDto> PinnedRepositories { get; set; }
    }
}