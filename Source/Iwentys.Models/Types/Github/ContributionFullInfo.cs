using System.Collections.Generic;
using System.Linq;

namespace Iwentys.Models.Types.Github
{
    public class ContributionFullInfo
    {
        public int Id { get; set; }
        public ActivityInfo RawActivity { get; set; }
        public List<ContributionsInfo> PerMonthActivity { get; set; }
        public int Total => PerMonthActivity.Sum(a => a.Count);
    }
}