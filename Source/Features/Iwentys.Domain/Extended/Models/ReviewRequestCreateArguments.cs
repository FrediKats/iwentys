using Iwentys.Domain.Extended.Enums;

namespace Iwentys.Domain.Extended.Models
{
    public class ReviewRequestCreateArguments
    {
        public long ProjectId { get; set; }
        public string Description { get; set; }
        public ProjectReviewVisibility Visibility { get; set; }
    }
}