using System.Collections.Generic;

namespace Iwentys.Domain.GithubIntegration.Models
{
    public class ActivityInfo
    {
        public List<YearActivityInfo> Years { get; set; }
        public List<ContributionsInfo> Contributions { get; set; }
    }
}