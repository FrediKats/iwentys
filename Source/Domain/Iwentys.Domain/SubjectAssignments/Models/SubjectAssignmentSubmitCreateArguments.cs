namespace Iwentys.Domain.SubjectAssignments.Models
{
    public class SubjectAssignmentSubmitCreateArguments
    {
        public int SubjectAssignmentId { get; set; }

        public string StudentDescription { get; set; }
        public string RepositoryOwner { get; set; }
        public string RepositoryName { get; set; }
    }
}