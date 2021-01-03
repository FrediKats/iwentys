using System;
using Iwentys.Features.Students.Models;
using Iwentys.Features.Study.SubjectAssignments.Entities;

namespace Iwentys.Features.Study.SubjectAssignments.Models
{
    public class SubjectAssignmentSubmitDto
    {
        public int Id { get; set; }
        public StudentInfoDto Student { get; set; }
        public string StudentDescription { get; set; }
        public DateTime SubmitTimeUtc { get; set; }
        public int SubjectAssignmentId { get; set; }
        public string SubjectAssignmentTitle { get; set; }

        public DateTime? ApproveTimeUtc { get; set; }
        public DateTime? RejectTimeUtc { get; set; }
        public string Comment { get; set; }


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
        }

        public SubjectAssignmentSubmitDto()
        {
        }
    }
}