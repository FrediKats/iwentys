using Iwentys.Features.PeerReview.Enums;

namespace Iwentys.Features.PeerReview.Models
{
    public class ReviewFeedbackCreateArguments
    {
        public string Description { get; set; }
        public ReviewFeedbackSummary Summary { get; set; }
    }
}