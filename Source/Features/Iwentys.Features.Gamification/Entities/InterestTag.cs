using System.Collections.Generic;

namespace Iwentys.Features.Gamification.Entities
{
    public class InterestTag
    {
        //TODO: support tags for companies

        public int Id { get; init; }
        public string Title { get; init; }

        public virtual ICollection<StudentInterestTag> UserInterestTags { get; init; }
    }
}