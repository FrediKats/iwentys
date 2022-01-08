using System;

namespace Iwentys.Domain.Assignments
{
    public record AssignmentCreateArguments
    {
        public AssignmentCreateArguments(string title, string description, int? subjectId, DateTime? deadlineTimeUtc, bool forStudyGroup) : this()
        {
            Title = title;
            Description = description;
            SubjectId = subjectId;
            DeadlineTimeUtc = deadlineTimeUtc;
            ForStudyGroup = forStudyGroup;
        }

        public AssignmentCreateArguments()
        {
        }

        public string Title { get; init; }
        public string Description { get; init; }
        public int? SubjectId { get; init; }
        public DateTime? DeadlineTimeUtc { get; init; }
        public bool ForStudyGroup { get; init; }
        public string Link { get; set; }
    }
}