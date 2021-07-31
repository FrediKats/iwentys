using System;
using System.ComponentModel.DataAnnotations;
using Iwentys.Domain.SubjectAssignments.Enums;

namespace Iwentys.Domain.SubjectAssignments.Models
{
    public class SubjectAssignmentCreateArguments
    {
        [Range(1, int.MaxValue, ErrorMessage = "Subject is not valid")]
        public int SubjectId { get; set; }
        [Required(AllowEmptyStrings=false, ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public string Description { get; set; }
        [RegularExpression(@"(https?:\/\/)?(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,4}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)",ErrorMessage = "Url is not valid")]
        public string Link { get; set; }
        [Required(AllowEmptyStrings=false, ErrorMessage = "Deadline is required")]
        public DateTime DeadlineUtc { get; set; }
        public int Position { get; set; }
        public AvailabilityState AvailabilityState { get; set; }
    }
}