using System;
using Iwentys.Features.Assignments.Entities;
using Iwentys.Features.Students.Models;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Assignments.Models
{
    public record AssignmentInfoDto(
        int Id,
        string Title,
        string Description,
        DateTime CreationTime,
        DateTime? Deadline,
        StudentInfoDto Creator,
        SubjectEntity Subject,
        bool IsCompeted)
    {
        public AssignmentInfoDto(AssignmentEntity assignment)
            : this(
                assignment.Id,
                assignment.Title,
                assignment.Description,
                assignment.CreationTime,
                assignment.Deadline,
                new StudentInfoDto(assignment.Creator),
                assignment.Subject,
                assignment.IsCompleted)
        {
        }

        public AssignmentInfoDto(StudentAssignmentEntity studentAssignment) : this(studentAssignment.Assignment)
        {
        }
    }
}