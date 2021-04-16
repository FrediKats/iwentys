using Iwentys.Domain.Enums;

namespace Iwentys.Domain.Models
{
    public class ReviewRequestCreateArguments
    {
        public long ProjectId { get; set; }
        public string Description { get; set; }
        public ProjectReviewVisibility Visibility { get; set; }
    }
}