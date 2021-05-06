using Iwentys.Domain.Study.Enums;

namespace Iwentys.Domain.Study.Models
{
    public class SubjectAssignmentSubmitFeedbackArguments
    {
        public int SubjectAssignmentSubmitId { get; set; }
        public string Comment { get; set; }
        public FeedbackType FeedbackType { get; set; }
    }
}