using System.Collections.Generic;

namespace Iwentys.Models.Types.Github
{
    public class ContributionFullInfo
    {
        public ActivityInfo RawActivity { get; set; }
        public List<ContributionsInfo> PerMonthActivity { get; set; }
    }
}