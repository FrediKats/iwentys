namespace Iwentys.Domain.GithubIntegration;

public class YearActivityInfo
{
    public int Id { get; set; }
    public string Year { get; set; }
    public int Total { get; set; }
    public int ActivityInfoId { get; set; }
    public CodingActivityInfo ActivityInfo { get; set; }
}