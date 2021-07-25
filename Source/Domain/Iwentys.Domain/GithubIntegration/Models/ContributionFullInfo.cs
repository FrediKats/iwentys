using System;
using System.Collections.Generic;
using System.Linq;

namespace Iwentys.Domain.GithubIntegration.Models
{
    public class ContributionFullInfo
    {
        public CodingActivityInfo RawActivity { get; set; }
        public int Total => PerMonthActivity().Sum(a => a.Count);

        public static ContributionFullInfo Empty => new ContributionFullInfo
        {
            RawActivity = new CodingActivityInfo
            {
                Contributions = new List<ContributionsInfo>(),
                Years = new List<YearActivityInfo>()
            }
        };

        public List<ContributionsInfo> PerMonthActivity()
        {
            return RawActivity
                .Contributions
                .Where(c => IsBelongToActivityPeriod(c.Date))
                .GroupBy(c => c.Date.AddDays(-c.Date.Day))
                .Take(12)
                .Select(c => new ContributionsInfo(c.Key, c.Sum(_ => _.Count)))
                .ToList();
        }

        public int GetActivityForPeriod(DateTime from, DateTime to)
        {
            return RawActivity
                .Contributions
                .Where(c => c.Date >= from && c.Date <= to)
                .Sum(c => c.Count);
        }

        public bool IsBelongToActivityPeriod(DateTime date)
        {
            DateTime currentMonthStart = DateTime.UtcNow.AddDays(-DateTime.UtcNow.Day);
            DateTime currentActivityPeriodStart = currentMonthStart.AddMonths(-11);

            return date >= currentActivityPeriodStart;
        }
    }
}