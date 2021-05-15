using System;
using Iwentys.Domain.Assignments.Enums;
using Iwentys.Domain.Study.Models;

namespace Iwentys.Domain.SubjectAssignments.Models
{
    public class SubjectAssignmentSubmitDto
    {
        public SubjectAssignmentSubmitDto(SubjectAssignmentSubmit submit) : this()
        {
            Id = submit.Id;
            Student = new StudentInfoDto(submit.Student);
            StudentDescription = submit.StudentDescription;
            SubmitTimeUtc = submit.SubmitTimeUtc;
            SubjectAssignmentId = submit.SubjectAssignmentId;
            SubjectAssignmentTitle = submit.SubjectAssignment.Title;
            ApproveTimeUtc = submit.ApproveTimeUtc;
            RejectTimeUtc = submit.RejectTimeUtc;
            Comment = submit.Comment;
            State = GetState();
        }

        public SubjectAssignmentSubmitDto()
        {
        }

        public int Id { get; set; }
        public StudentInfoDto Student { get; set; }
        public string StudentDescription { get; set; }
        public DateTime SubmitTimeUtc { get; set; }
        public int SubjectAssignmentId { get; set; }
        public string SubjectAssignmentTitle { get; set; }

        public DateTime? ApproveTimeUtc { get; set; }
        public DateTime? RejectTimeUtc { get; set; }
        public string Comment { get; set; }
        public AssignmentSubmitState State { get; set; }


        public AssignmentSubmitState GetState()
        {
            if (RejectTimeUtc is not null)
                return AssignmentSubmitState.Rejected;
            if (ApproveTimeUtc is not null)
                return AssignmentSubmitState.Approved;
            return AssignmentSubmitState.Open;
        }
    }
}