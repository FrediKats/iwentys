using System;
using System.Globalization;

namespace Iwentys.Domain.GithubIntegration
{
    public class CodingActivityInfoResponse
    {
        public DateTime Date { get; set; }
        public string Month { get; set; }
        public int Activity { get; set; }

        public static CodingActivityInfoResponse Wrap(ContributionsInfo contributions)
        {
            return new CodingActivityInfoResponse
            {
                Date = contributions.Date,
                Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(contributions.Date.Month),
                Activity = contributions.Count
            };
        }
    }
}