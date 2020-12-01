namespace Iwentys.Integrations.GithubIntegration.Models
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

        public GithubRepository()
        {
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int StarCount { get; set; }
    }
}