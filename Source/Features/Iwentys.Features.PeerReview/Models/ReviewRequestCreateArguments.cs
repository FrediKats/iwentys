using Iwentys.Features.PeerReview.Enums;

namespace Iwentys.Features.PeerReview.Models
{
    public class ReviewRequestCreateArguments
    {
        public long ProjectId { get; set; }
        public string Description { get; set; }
        public ProjectReviewVisibility Visibility { get; set; }
    }
}