using Iwentys.Features.GithubIntegration.Models;

namespace Iwentys.Features.Guilds.Entities
{
    public class GuildPinnedProject
    {
        //FYI: It's always must be github repo id
        public long Id { get; init; }

        public virtual Guild Guild { get; init; }
        public int GuildId { get; init; }

        public string RepositoryOwner { get; init; }
        public string RepositoryName { get; init; }


        public static GuildPinnedProject Create(int guildId, GithubRepositoryInfoDto repositoryInfoDto)
        {
            return new GuildPinnedProject
            {
                Id = repositoryInfoDto.Id,
                GuildId = guildId,
                RepositoryName = repositoryInfoDto.Name,
                RepositoryOwner = repositoryInfoDto.Owner
            };
        }
    }
}