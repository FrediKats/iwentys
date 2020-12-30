using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.GithubIntegration.Entities
{
    public class GithubProject
    {
        public GithubProject()
        {
        }

        public GithubProject(Student owner, GithubRepositoryInfoDto githubRepositoryInfoDto) : this()
        {
            Id = githubRepositoryInfoDto.Id;
            Owner = owner.GithubUsername;
            Description = githubRepositoryInfoDto.Description;
            FullUrl = githubRepositoryInfoDto.Url;
            Name = githubRepositoryInfoDto.Name;
            StudentId = owner.Id;
        }

        public long Id { get; set; }
        public string FullUrl { get; set; }
        public string Owner { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StarCount { get; set; }
        public long GithubRepositoryId { get; set; }

        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
    }
}