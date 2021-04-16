using Iwentys.Domain.Models;

namespace Iwentys.Domain
{
    public class GithubProject
    {
        public GithubProject()
        {
        }

        public GithubProject(GithubUser githubUser, GithubRepositoryInfoDto githubRepositoryInfoDto) : this()
        {
            Id = githubRepositoryInfoDto.Id;
            Owner = githubUser.Username;
            Description = githubRepositoryInfoDto.Description;
            FullUrl = githubRepositoryInfoDto.Url;
            Name = githubRepositoryInfoDto.Name;
            OwnerUserId = githubUser.IwentysUserId;
        }

        public long Id { get; set; }
        public string FullUrl { get; set; }
        public string Owner { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StarCount { get; set; }

        public int? OwnerUserId { get; set; }
        public virtual IwentysUser OwnerUser { get; set; }
    }
}