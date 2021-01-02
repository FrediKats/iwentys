using Iwentys.Features.Study.SubjectAssignments.Enums;

namespace Iwentys.Features.Study.SubjectAssignments.Models
{
    public class SubjectAssignmentSubmitFeedbackArguments
    {
        public string Comment { get; set; }
        public FeedbackType FeedbackType { get; set; }
    }
}