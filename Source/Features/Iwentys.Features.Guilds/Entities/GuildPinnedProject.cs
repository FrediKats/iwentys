using System.ComponentModel.DataAnnotations.Schema;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.GithubIntegration.Models;

namespace Iwentys.Features.Guilds.Entities
{
    public class GuildPinnedProject
    {
        public long Id { get; init; }

        [ForeignKey(nameof(Id))]
        public virtual GithubProject Project { get; set; }

        public virtual Guild Guild { get; init; }
        public int GuildId { get; init; }

        public static GuildPinnedProject Create(int guildId, GithubRepositoryInfoDto repositoryInfoDto)
        {
            return new GuildPinnedProject
            {
                Id = repositoryInfoDto.Id,
                GuildId = guildId
            };
        }
    }
}