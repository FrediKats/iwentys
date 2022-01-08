using System;
using System.Linq.Expressions;

namespace Iwentys.Domain.GithubIntegration
{
    public record GithubRepositoryInfoDto
    {
        public GithubRepositoryInfoDto(GithubProject entity)
            : this(
                entity.Id,
                entity.Owner,
                entity.Name,
                entity.Description,
                entity.FullUrl,
                entity.StarCount)
        {
        }

        public GithubRepositoryInfoDto(long id, string owner, string name, string description, string url, int starCount)
        {
            Id = id;
            Owner = owner;
            Name = name;
            Description = description;
            Url = url;
            StarCount = starCount;
        }

        public GithubRepositoryInfoDto()
        {
        }

        public static Expression<Func<GithubProject, GithubRepositoryInfoDto>> FromEntity =>
            githubProject => new GithubRepositoryInfoDto
            {
                Id = githubProject.Id,
                Owner = githubProject.Owner,
                Name = githubProject.Name,
                Description = githubProject.Description,
                Url = githubProject.FullUrl,
                StarCount = githubProject.StarCount
            };

        public long Id { get; init; }
        public string Owner { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public string Url { get; init; }
        public int StarCount { get; init; }

        public string GithubLikeTitle()
        {
            return $"{Owner}/{Name}";
        }
    }
}