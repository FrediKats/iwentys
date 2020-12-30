using Iwentys.Features.GithubIntegration.Models;

namespace Iwentys.Features.Guilds.Entities
{
    public class GuildPinnedProject
    {
        //FYI: It's always must be github repo id
        public long Id { get; set; }

        public virtual Guild Guild { get; set; }
        public int GuildId { get; set; }

        public string RepositoryOwner { get; set; }
        public string RepositoryName { get; set; }


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