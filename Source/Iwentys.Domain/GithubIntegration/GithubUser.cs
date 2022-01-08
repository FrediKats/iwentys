using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Iwentys.Common;
using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.GithubIntegration
{
    public class GithubUser
    {
        [Key] public int IwentysUserId { get; set; }

        public string Username { get; set; }
        public string AvatarUrl { get; set; }
        public string Bio { get; set; }
        public string Company { get; set; }
        public string SerializedContributionData { get; set; }

        public virtual ICollection<GithubProject> Projects { get; set; }

        [NotMapped]
        public ContributionFullInfo ContributionFullInfo
        {
            get => SerializedContributionData.Maybe(data => JsonSerializer.Deserialize<ContributionFullInfo>(data));
            set => SerializedContributionData = JsonSerializer.Serialize(value);
        }

        public static GithubUser Create(IwentysUser user, GithubUserInfoDto githubUser, ContributionFullInfo contributionFullInfo)
        {
            return new GithubUser
            {
                IwentysUserId = user.Id,
                Username = user.GithubUsername,
                AvatarUrl = githubUser.AvatarUrl,
                Bio = githubUser.Bio,
                Company = githubUser.Bio,
                ContributionFullInfo = contributionFullInfo
            };
        }

        public void Update(GithubUserInfoDto githubUser, ContributionFullInfo contributionFullInfo)
        {
            AvatarUrl = githubUser.AvatarUrl;
            Bio = githubUser.Bio;
            Company = githubUser.Company;
            ContributionFullInfo = contributionFullInfo;
        }
    }
}