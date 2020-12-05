using System;

namespace Iwentys.Features.Assignments.Models
{
    public record AssignmentCreateRequestDto(string Title, string Description, int? SubjectId, DateTime? Deadline)
    {
    }
}