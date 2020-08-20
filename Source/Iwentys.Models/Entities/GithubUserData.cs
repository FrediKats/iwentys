using Iwentys.Models.Entities.Github;

namespace Iwentys.Models.Entities
{
    public class GithubUserData
    {
        public Student Student { get; set; }
        public int StudentId { get; set; }

        public int Id { get; set; }
        public int GithubUserId { get; set; }
        public GithubUser GithubUser { get; set; }
        public ContributionFullInfo ContributionFullInfo { get; set; }
    }
}
