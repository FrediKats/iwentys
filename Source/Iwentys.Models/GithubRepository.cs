using Iwentys.Models.Entities.Github;

namespace Iwentys.Models
{
    public class GithubRepository
    {
        public GithubRepository(long id, string name, string description, string url, int starCount) : this()
        {
            Id = id;
            Name = name;
            Description = description;
            Url = url;
            StarCount = starCount;
        }

        public GithubRepository(GithubProjectEntity projectEntity) : this()
        {
            Id = projectEntity.GithubRepositoryId;
            Name = projectEntity.Name;
            Description = projectEntity.Description;
            Url = projectEntity.FullUrl;
            StarCount = projectEntity.StarCount;
        }

        private GithubRepository()
        {
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int StarCount { get; set; }
    }
}