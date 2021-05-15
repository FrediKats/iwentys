using System;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.SubjectAssignments.Enums;
using Iwentys.Domain.SubjectAssignments.Models;

namespace Iwentys.Domain.SubjectAssignments
{
    public class SubjectAssignmentSubmit
    {
        public int Id { get; set; }

        public int SubjectAssignmentId { get; set; }
        public virtual SubjectAssignment SubjectAssignment { get; set; }

        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        public DateTime SubmitTimeUtc { get; set; }
        public string StudentDescription { get; set; }

        public DateTime? ApproveTimeUtc { get; set; }
        public DateTime? RejectTimeUtc { get; set; }
        public string Comment { get; set; }

        public void ApplyFeedback(IwentysUser iwentysUser, SubjectAssignmentSubmitFeedbackArguments arguments)
        {
            SubjectTeacher teacher = iwentysUser.EnsureIsTeacher(SubjectAssignment.Subject);

            switch (arguments.FeedbackType)
            {
                case FeedbackType.Approve:
                    Approve(teacher, arguments);
                    break;
                case FeedbackType.Reject:
                    Reject(teacher, arguments);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(FeedbackType), "Unsupported feedback state");
            }
        }

        private void Approve(SubjectTeacher teacher, SubjectAssignmentSubmitFeedbackArguments arguments)
        {
            RejectTimeUtc = null;
            ApproveTimeUtc = DateTime.UtcNow;
            Comment = arguments.Comment;
        }

        private void Reject(SubjectTeacher teacher, SubjectAssignmentSubmitFeedbackArguments arguments)
        {
            ApproveTimeUtc = null;
            RejectTimeUtc = DateTime.UtcNow;
            Comment = arguments.Comment;
        }
    }
}