namespace Iwentys.Domain.SubjectAssignments.Models
{
    public class SubjectAssignmentSubmitSearchArguments
    {
        public int? SubjectId { get; set; }
        public int? SubjectAssignmentId { get; set; }
        public int? GroupId { get; set; }
        public int? StudentId { get; set; }

        public bool SkipApproved { get; set; }
        public bool SkipRejected { get; set; }
        public bool SkipNotChecked { get; set; }
    }
}