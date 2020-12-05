using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.GithubIntegration.Entities
{
    public class GithubProjectEntity
    {
        public GithubProjectEntity()
        {
        }

        public GithubProjectEntity(StudentEntity owner, GithubRepositoryInfoDto githubRepositoryInfoDto) : this()
        {
            Id = githubRepositoryInfoDto.Id;
            Owner = owner.GithubUsername;
            Description = githubRepositoryInfoDto.Description;
            FullUrl = githubRepositoryInfoDto.Url;
            Name = githubRepositoryInfoDto.Name;
            Student = owner;
            StudentId = owner.Id;
        }

        public int StudentId { get; set; }
        public StudentEntity Student { get; set; }

        public long Id { get; set; }
        public string FullUrl { get; set; }
        public string Owner { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StarCount { get; set; }
        public long GithubRepositoryId { get; set; }
        public string UserName { get; set; }
    }
}