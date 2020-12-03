using System;

namespace Iwentys.Features.Assignments.ViewModels
{
    public class AssignmentCreateRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int? SubjectId { get; set; }
        public DateTime? Deadline { get; set; }
    }
}