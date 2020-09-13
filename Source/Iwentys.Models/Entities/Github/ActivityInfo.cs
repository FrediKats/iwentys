namespace Iwentys.Models.Entities.Github
{
    public class ActivityInfo
    {
        public int Id { get; set; }
        public YearActivityInfo[] Years { get; set; }
        public ContributionsInfo[] Contributions { get; set; }
    }
}