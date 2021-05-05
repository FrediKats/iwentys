using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Iwentys.Domain.GithubIntegration.Models
{
    public class ContributionFullInfo
    {
        public ActivityInfo RawActivity { get; set; }
        public int Total => PerMonthActivity().Sum(a => a.Count);

        public static ContributionFullInfo Empty => new ContributionFullInfo
        {
            RawActivity = new ActivityInfo
            {
                Contributions = new List<ContributionsInfo>(),
                Years = new List<YearActivityInfo>()
            }
        };

        public List<ContributionsInfo> PerMonthActivity()
        {
            //TODO: convert date to month name
            return RawActivity
                .Contributions
                .GroupBy(c => c.Date.Substring(0, 7))
                .Take(12)
                .Select(c => new ContributionsInfo(c.Key, c.Sum(_ => _.Count)))
                .ToList();
        }

        public int GetActivityForPeriod(DateTime from, DateTime to)
        {
            return RawActivity
                .Contributions
                .Select(c => (Date: DateTime.Parse(c.Date, CultureInfo.InvariantCulture), c.Count))
                .Where(c => c.Date >= from && c.Date <= to)
                .Sum(c => c.Count);
        }
    }
}