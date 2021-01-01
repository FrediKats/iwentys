using System;

namespace Iwentys.Features.Assignments.Models
{
    public record AssignmentCreateRequestDto
    {
        public AssignmentCreateRequestDto(string title, string description, int? subjectId, DateTime? deadline, bool forStudyGroup) : this()
        {
            Title = title;
            Description = description;
            SubjectId = subjectId;
            Deadline = deadline;
            ForStudyGroup = forStudyGroup;
        }

        public AssignmentCreateRequestDto()
        {
        }
        
        public string Title { get; init; }
        public string Description { get; init; }
        public int? SubjectId { get; init; }
        public DateTime? Deadline { get; init; }
        public bool ForStudyGroup { get; init; }
    }
}