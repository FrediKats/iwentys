using System.ComponentModel.DataAnnotations;

namespace Iwentys.Domain.SubjectAssignments.Models
{
    public class SubjectAssignmentSubmitCreateArguments
    {
        [Range(1, int.MaxValue, ErrorMessage = "Subject Assignment is not valid")]
        public int SubjectAssignmentId { get; set; }
        public string StudentDescription { get; set; }
        public string RepositoryOwner { get; set; }
        public string RepositoryName { get; set; }
    }
}