using System;

namespace Iwentys.Features.Study.SubjectAssignments.Models
{
    public class SubjectAssignmentCreateArguments
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime DeadlineUtc { get; set; }
    }
}