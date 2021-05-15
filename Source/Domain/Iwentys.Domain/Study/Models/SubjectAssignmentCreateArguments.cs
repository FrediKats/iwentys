using System;
using Iwentys.Domain.Assignments.Models;

namespace Iwentys.Domain.Study.Models
{
    public class SubjectAssignmentCreateArguments
    {
        public int SubjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime DeadlineUtc { get; set; }

        public AssignmentCreateArguments ConvertToAssignmentCreateArguments(int subjectId)
        {
            return new AssignmentCreateArguments
            {
                Title = Title,
                Description = Description,
                DeadlineTimeUtc = DeadlineUtc,
                SubjectId = subjectId,
                Link = Link
            };
        }
    }
}