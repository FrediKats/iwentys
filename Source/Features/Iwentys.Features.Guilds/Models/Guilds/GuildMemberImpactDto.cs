using Iwentys.Features.GithubIntegration.Entities;

namespace Iwentys.Features.Guilds.Models.Guilds
{
    public record GuildMemberImpactDto
    {
        public GuildMemberImpactDto(GithubUserEntity userEntity)
            : this(userEntity.Username, userEntity.ContributionFullInfo?.Total ?? 0)
        {
        }

        public GuildMemberImpactDto(string username, int totalRate)
        {
            Username = username;
            TotalRate = totalRate;
        }

        public GuildMemberImpactDto()
        {
        }
        
        public string Username { get; init; }
        public int TotalRate { get; init; }
    }
}