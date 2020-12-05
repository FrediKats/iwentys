using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Iwentys.Common.Tools;
using Iwentys.Features.GithubIntegration.ViewModels;
using Newtonsoft.Json;

namespace Iwentys.Features.GithubIntegration.Entities
{
    public class GithubUserEntity
    {
        [Key] public int StudentId { get; set; }

        public string Username { get; set; }
        public string AvatarUrl { get; set; }
        public string Bio { get; set; }
        public string Company { get; set; }
        public string SerializedContributionData { get; set; }

        [NotMapped]
        public ContributionFullInfo ContributionFullInfo
        {
            get => SerializedContributionData.Maybe(JsonConvert.DeserializeObject<ContributionFullInfo>);
            set => SerializedContributionData = JsonConvert.SerializeObject(value);
        }
    }
}