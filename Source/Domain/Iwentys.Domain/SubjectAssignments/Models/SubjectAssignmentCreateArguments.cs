using System;
using Iwentys.Domain.SubjectAssignments.Enums;

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
        public AvailabilityState AvailabilityState { get; set; }
    }
}