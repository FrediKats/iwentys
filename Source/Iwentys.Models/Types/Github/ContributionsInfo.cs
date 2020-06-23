namespace Iwentys.Models.Types.Github
{
    public class ContributionsInfo
    {
        public string Date { get; set; }
        public int Count { get; set; }

        public ContributionsInfo(string date, int count) : this()
        {
            Date = date;
            Count = count;
        }

        private ContributionsInfo()
        {
        }
    }
}