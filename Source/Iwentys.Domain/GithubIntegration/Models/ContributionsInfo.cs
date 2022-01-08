using System;

namespace Iwentys.Domain.GithubIntegration;

public class ContributionsInfo
{
    public ContributionsInfo(DateTime date, int count) : this()
    {
        Date = date;
        Count = count;
    }

    private ContributionsInfo()
    {
    }

    public DateTime Date { get; set; }
    public int Count { get; set; }
}