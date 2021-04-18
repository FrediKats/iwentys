using Iwentys.Domain.Extended.Enums;

namespace Iwentys.Domain.Extended.Models
{
    public class ReviewFeedbackCreateArguments
    {
        public string Description { get; set; }
        public ReviewFeedbackSummary Summary { get; set; }
    }
}