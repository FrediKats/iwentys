using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Iwentys.Common.Tools;
using Iwentys.Features.GithubIntegration.Models;

namespace Iwentys.Features.GithubIntegration.Entities
{
    public class GithubUserEntity
    {
        [Key]
        public int StudentId { get; set; }

        public string Username { get; set; }
        public string AvatarUrl { get; set; }
        public string Bio { get; set; }
        public string Company { get; set; }
        public string SerializedContributionData { get; set; }

        [NotMapped]
        public ContributionFullInfo ContributionFullInfo
        {
            get => SerializedContributionData.Maybe(data => JsonSerializer.Deserialize<ContributionFullInfo>(data));
            set => SerializedContributionData = JsonSerializer.Serialize(value);
        }
    }
}