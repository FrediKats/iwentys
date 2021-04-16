using Iwentys.Domain.Study.Enums;

namespace Iwentys.Domain.Models
{
    public class SubjectAssignmentSubmitFeedbackArguments
    {
        public string Comment { get; set; }
        public FeedbackType FeedbackType { get; set; }
    }
}