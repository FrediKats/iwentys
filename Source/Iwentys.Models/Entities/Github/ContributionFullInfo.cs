using System.Collections.Generic;
using System.Linq;

namespace Iwentys.Models.Entities.Github
{
    public class ContributionFullInfo
    {
        public int Id { get; set; }
        public ActivityInfo RawActivity { get; set; }
        public int Total => PerMonthActivity().Sum(a => a.Count);

        public List<ContributionsInfo> PerMonthActivity()
        {
            return RawActivity
                    .Contributions
                    .GroupBy(c => c.Date.Substring(0, 7))
                    .Select(c => new ContributionsInfo(c.Key, c.Sum(_ => _.Count)))
                    .ToList();
        }
    }
}