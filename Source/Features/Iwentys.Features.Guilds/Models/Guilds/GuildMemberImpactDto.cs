using Iwentys.Features.GithubIntegration.Entities;

namespace Iwentys.Features.Guilds.Models.Guilds
{
    public record GuildMemberImpactDto(string Username, int TotalRate)
    {
        public GuildMemberImpactDto(GithubUserEntity userEntity)
            : this(userEntity.Username, userEntity.ContributionFullInfo?.Total ?? 0)
        {
        }

        public GuildMemberImpactDto() : this(default, default)
        {
        }
    }
}