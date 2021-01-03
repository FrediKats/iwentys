using System.Collections.Generic;

namespace Iwentys.Features.Gamification.Entities
{
    public class InterestTag
    {
        public int Id { get; init; }
        public string Title { get; init; }

        public virtual ICollection<StudentInterestTag> UserInterestTags { get; init; }
    }
}