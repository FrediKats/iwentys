using Iwentys.Domain.Enums;

namespace Iwentys.Domain.Models
{
    public class ReviewFeedbackCreateArguments
    {
        public string Description { get; set; }
        public ReviewFeedbackSummary Summary { get; set; }
    }
}