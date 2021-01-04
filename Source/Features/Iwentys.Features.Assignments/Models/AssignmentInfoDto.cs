using System;
using System.Linq.Expressions;
using Iwentys.Features.AccountManagement.Models;
using Iwentys.Features.Assignments.Entities;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Assignments.Models
{
    public record AssignmentInfoDto
    {
        public AssignmentInfoDto(int id, string title, string description, DateTime creationTime, DateTime? deadline, IwentysUserInfoDto creator, Subject subject, bool isCompeted)
        {
            Id = id;
            Title = title;
            Description = description;
            CreationTime = creationTime;
            Deadline = deadline;
            Creator = creator;
            Subject = subject;
            IsCompeted = isCompeted;
        }

        public AssignmentInfoDto(StudentAssignment studentAssignment)
            : this(
                studentAssignment.Assignment.Id,
                studentAssignment.Assignment.Title,
                studentAssignment.Assignment.Description,
                studentAssignment.Assignment.CreationTimeUtc,
                studentAssignment.Assignment.DeadlineTimeUtc,
                new IwentysUserInfoDto(studentAssignment.Assignment.Author),
                studentAssignment.Assignment.Subject,
                studentAssignment.IsCompleted)
        {
        }

        public AssignmentInfoDto()
        {
        }

        public static Expression<Func<StudentAssignment, AssignmentInfoDto>> FromStudentEntity => entity => new AssignmentInfoDto(entity);

        public int Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public DateTime CreationTime { get; init; }
        public DateTime? Deadline { get; init; }
        public IwentysUserInfoDto Creator { get; init; }
        public Subject Subject { get; init; }
        public bool IsCompeted { get; init; }
    }
}