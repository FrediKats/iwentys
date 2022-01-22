using System;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;

namespace Iwentys.Domain.SubjectAssignments;

public class SubjectAssignmentSubmit
{
    public int Id { get; set; }
    public DateTime SubmitTimeUtc { get; set; }
    public string StudentDescription { get; set; }
    public string StudentPRLink { get; set; }
    public string RepositoryOwner { get; set; }
    public string RepositoryName { get; set; }

    public string Comment { get; set; }
    public int ReviewerId { get; set; }
    public virtual IwentysUser Reviewer { get; set; }
    //TODO: validate range
    public int Points { get; set; }
    public DateTime? ApproveTimeUtc { get; set; }
    public DateTime? RejectTimeUtc { get; set; }

    public int SubjectAssignmentId { get; set; }
    public virtual SubjectAssignment SubjectAssignment { get; set; }

    public int StudentId { get; set; }
    public virtual Student Student { get; set; }

    public SubmitState State
    {
        get
        {
            if (ApproveTimeUtc is not null)
                return SubmitState.Approved;
            if (RejectTimeUtc is not null)
                return SubmitState.Rejected;
            return SubmitState.Created;
        }
    }

    public SubjectAssignmentSubmit()
    {
    }

    public SubjectAssignmentSubmit(Student student, SubjectAssignment subjectAssignment, SubjectAssignmentSubmitCreateArguments arguments) : this()
    {
        Student = student;
        StudentId = student.Id;
        SubjectAssignment = subjectAssignment;
        SubjectAssignmentId = subjectAssignment.Id;
        SubmitTimeUtc = DateTime.UtcNow;
        StudentDescription = arguments.StudentDescription;
        StudentPRLink = arguments.StudentPRLink;
        RepositoryOwner = arguments.RepositoryOwner;
        RepositoryName = arguments.RepositoryName;
    }

    public void AddFeedback(IwentysUser iwentysUser, SubjectAssignmentSubmitFeedbackArguments arguments)
    {
        ReviewerId = iwentysUser.Id;
        Reviewer = iwentysUser;
            
        switch (arguments.FeedbackType)
        {
            case FeedbackType.Approve:
                Approve(arguments);
                break;
            case FeedbackType.Reject:
                Reject(arguments);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(FeedbackType), "Unsupported feedback state");
        }
    }

    private void Approve(SubjectAssignmentSubmitFeedbackArguments arguments)
    {
        RejectTimeUtc = null;
        ApproveTimeUtc = DateTime.UtcNow;
        Comment = arguments.Comment;
        Points = arguments.Points ?? throw new Exception($"Argument is not provided: {nameof(arguments.Points)}");
    }

    private void Reject(SubjectAssignmentSubmitFeedbackArguments arguments)
    {
        ApproveTimeUtc = null;
        RejectTimeUtc = DateTime.UtcNow;
        Comment = arguments.Comment;
    }
}