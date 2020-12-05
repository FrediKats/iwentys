using Iwentys.Features.GithubIntegration.Entities;

namespace Iwentys.Features.GithubIntegration.Models
{
    public record GithubRepositoryInfoDto(
        long Id,
        string Owner,
        string Name,
        string Description,
        string Url,
        int StarCount)
    {
        public GithubRepositoryInfoDto(GithubProjectEntity entity)
        : this(
            entity.Id,
            entity.Owner,
            entity.Name,
            entity.Description,
            entity.FullUrl,
            entity.StarCount)
        {
        }
    }
}