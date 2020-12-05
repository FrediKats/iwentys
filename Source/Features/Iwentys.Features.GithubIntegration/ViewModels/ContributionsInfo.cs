namespace Iwentys.Features.GithubIntegration.ViewModels
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
        public int ActivityInfoId { get; set; }
        public ActivityInfo ActivityInfo { get; set; }
    }
}