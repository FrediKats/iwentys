using System;
using System.ComponentModel.DataAnnotations;
using Iwentys.Domain.SubjectAssignments.Enums;

namespace Iwentys.Domain.SubjectAssignments.Models
{
    public class SubjectAssignmentUpdateArguments
    {
        [Range(1, int.MaxValue, ErrorMessage = "Subject Assignment is not valid")]
        public int SubjectAssignmentId { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        [Required(ErrorMessage = "Deadline is required")]
        public DateTime DeadlineUtc { get; set; }
        public int Position { get; set; }
        public AvailabilityState AvailabilityState { get; set; }
    }
}