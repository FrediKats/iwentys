namespace Iwentys.Models.Entities.Github
{
    public class GithubProjectEntity
    {
        public GithubProjectEntity()
        {
        }

        public GithubProjectEntity(StudentEntity owner, GithubRepository githubRepository) : this()
        {
            Id = githubRepository.Id;
            Author = owner.GithubUsername;
            Description = githubRepository.Description;
            FullUrl = githubRepository.Url;
            Name = githubRepository.Name;
            Student = owner;
            StudentId = owner.Id;
        }

        public int StudentId { get; set; }
        public StudentEntity Student { get; set; }

        public long Id { get; set; }
        public string FullUrl { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StarCount { get; set; }
        public long GithubRepositoryId { get; set; }
        public string UserName { get; set; }
    }
}