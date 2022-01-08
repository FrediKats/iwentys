namespace Iwentys.Domain.PeerReview;

public class ReviewRequestCreateArguments
{
    public long ProjectId { get; set; }
    public string Description { get; set; }
    public ProjectReviewVisibility Visibility { get; set; }
}