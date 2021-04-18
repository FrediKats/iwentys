using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain
{
    public class ProjectReviewRequestInvite
    {
        public int ReviewRequestId { get; set; }
        public virtual ProjectReviewRequest ReviewRequest { get; set; }

        public int ReviewerId { get; set; }
        public virtual IwentysUser Reviewer { get; set; }
    }
}