using System;
using Iwentys.Domain.Study.Models;
using Iwentys.Domain.SubjectAssignments;
using Iwentys.Domain.SubjectAssignments.Enums;

namespace Iwentys.Modules.SubjectAssignments.Dtos
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
            Points = submit.Points;
            ReviewerId = submit.ReviewerId;
            ApproveTimeUtc = submit.ApproveTimeUtc;
            RejectTimeUtc = submit.RejectTimeUtc;
            Comment = submit.Comment;
            State = submit.State;
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
        public int ReviewerId { get; set; }
        public int Points { get; set; }
        public DateTime? ApproveTimeUtc { get; set; }
        public DateTime? RejectTimeUtc { get; set; }
        public string Comment { get; set; }
        public SubmitState State { get; set; }
    }
}