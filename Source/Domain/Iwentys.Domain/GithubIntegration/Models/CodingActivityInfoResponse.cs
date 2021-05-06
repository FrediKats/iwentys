namespace Iwentys.Domain.GithubIntegration.Models
{
    public class CodingActivityInfoResponse
    {
        public string Month { get; set; }
        public int Activity { get; set; }

        public static CodingActivityInfoResponse Wrap(ContributionsInfo contributions)
        {
            return new CodingActivityInfoResponse
            {
                Month = contributions.Date,
                Activity = contributions.Count
            };
        }
    }
}