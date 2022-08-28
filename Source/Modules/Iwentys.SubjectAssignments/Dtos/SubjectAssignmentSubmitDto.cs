using System;
using Iwentys.Domain.SubjectAssignments;
using Iwentys.EntityManager.ApiClient;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;

namespace Iwentys.SubjectAssignments;

public class SubjectAssignmentSubmitDto
{
    public SubjectAssignmentSubmitDto(SubjectAssignmentSubmit submit) : this()
    {
        Id = submit.Id;
        Student = EntityManagerApiDtoMapper.Map(submit.Student);
        StudentDescription = submit.StudentDescription;
        StudentPRLink = submit.StudentPRLink;
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
    public string StudentPRLink { get; set; }
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