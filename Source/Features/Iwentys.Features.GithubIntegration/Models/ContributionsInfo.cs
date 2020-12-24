namespace Iwentys.Features.GithubIntegration.Models
{
    public class ContributionsInfo
    {
        public ContributionsInfo(string date, int count) : this()
        {
            Date = date;
            Count = count;
        }

        private ContributionsInfo()
        {
        }

        public int Id { get; set; }
        public string Date { get; set; }
        public int Count { get; set; }
    }
}