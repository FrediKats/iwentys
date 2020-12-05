using System;

namespace Iwentys.Features.Assignments.Models
{
    public class AssignmentCreateRequestDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int? SubjectId { get; set; }
        public DateTime? Deadline { get; set; }
    }
}