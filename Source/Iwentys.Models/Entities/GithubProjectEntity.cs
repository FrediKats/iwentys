using Iwentys.Models.Entities.Github;

namespace Iwentys.Models.Entities
{
    public class GithubProjectEntity
    {
        public Student Student { get; set; }
        public int StudentId { get; set; }

        public long Id { get; set; }
        public string FullUrl { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StarCount { get; set; }
        public long GithubRepositoryId { get; set; }
        public string UserName { get; set; }

        public GithubProjectEntity()
        {
        }

        public GithubProjectEntity(Student owner, GithubRepository githubRepository) : this()
        {
            Id = githubRepository.Id;
            Author = owner.GithubUsername;
            Description = githubRepository.Description;
            FullUrl = githubRepository.Url;
            Name = githubRepository.Name;
            Student = owner;
            StudentId = owner.Id;
        }
    }
}