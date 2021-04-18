using Iwentys.Domain.Study.Enums;

namespace Iwentys.Domain.Study.Models
{
    public class SubjectAssignmentSubmitFeedbackArguments
    {
        public string Comment { get; set; }
        public FeedbackType FeedbackType { get; set; }
    }
}