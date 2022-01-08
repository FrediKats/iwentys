using System;
using System.ComponentModel.DataAnnotations;

namespace Iwentys.Domain.SubjectAssignments
{
    public class SubjectAssignmentUpdateArguments
    {
        [Range(1, int.MaxValue, ErrorMessage = "Subject Assignment is not valid")]
        public int SubjectAssignmentId { get; set; }
        [Required(AllowEmptyStrings=false,ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime? DeadlineUtc { get; set; }
        public int Position { get; set; }
        public AvailabilityState AvailabilityState { get; set; }
    }
}