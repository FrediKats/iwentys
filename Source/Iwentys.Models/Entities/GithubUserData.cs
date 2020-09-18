using System.ComponentModel.DataAnnotations;
using Iwentys.Models.Entities.Github;

namespace Iwentys.Models.Entities
{
    public class GithubUserData
    {
        [Key]
        public int StudentId { get; set; }
        public string Username { get; set; }
        public string AvatarUrl { get; set; }
        public string Bio { get; set; }
        public string Company { get; set; }
        public ContributionFullInfo ContributionFullInfo { get; set; }
    }
}
