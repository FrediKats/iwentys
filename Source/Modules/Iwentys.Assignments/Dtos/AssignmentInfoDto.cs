using System;
using System.Linq.Expressions;
using Iwentys.AccountManagement;
using Iwentys.Domain.Assignments;
using Iwentys.Domain.Study;
using Iwentys.EntityManager.ApiClient;
using Iwentys.WebService.Application;

namespace Iwentys.Assignments;

public record AssignmentInfoDto
{
    public AssignmentInfoDto(int id, string title, string description, DateTime creationTimeUtc, DateTime? deadlineTimeUtc, IwentysUserInfoDto creator, Subject subject, bool isCompeted)
    {
        Id = id;
        Title = title;
        Description = description;
        CreationTimeUtc = creationTimeUtc;
        DeadlineTimeUtc = deadlineTimeUtc;
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
            EntityManagerApiDtoMapper.Map(studentAssignment.Assignment.Author),
            studentAssignment.Assignment.Subject,
            studentAssignment.IsCompleted)
    {
    }

    public AssignmentInfoDto()
    {
    }

    public static Expression<Func<StudentAssignment, AssignmentInfoDto>> FromStudentEntity =>
        entity => new AssignmentInfoDto
        {
            Id = entity.Assignment.Id,
            Title = entity.Assignment.Title,
            Description = entity.Assignment.Description,
            CreationTimeUtc = entity.Assignment.CreationTimeUtc,
            DeadlineTimeUtc = entity.Assignment.DeadlineTimeUtc,
            Creator = EntityManagerApiDtoMapper.Map(entity.Assignment.Author),
            IsCompeted = entity.IsCompleted
        };

    public int Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public DateTime CreationTimeUtc { get; init; }
    public DateTime? DeadlineTimeUtc { get; init; }
    public IwentysUserInfoDto Creator { get; init; }
    public Subject Subject { get; init; }
    public bool IsCompeted { get; init; }
}