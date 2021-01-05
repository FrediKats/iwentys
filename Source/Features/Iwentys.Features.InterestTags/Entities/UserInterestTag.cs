using Iwentys.Features.AccountManagement.Entities;

namespace Iwentys.Features.InterestTags.Entities
{
    public class UserInterestTag
    {
        public int InterestTagId { get; init; }
        public virtual InterestTag InterestTag { get; init; }

        public int UserId { get; init; }
        public virtual IwentysUser User { get; init; }
    }
}