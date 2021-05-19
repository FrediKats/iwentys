using System;

namespace Iwentys.Domain.SubjectAssignments.Models
{
    public class SubjectAssignmentCreateArguments
    {
        public int SubjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime DeadlineUtc { get; set; }
        public int Position { get; set; }
        public bool AvailableForStudent { get; set; }
    }
}