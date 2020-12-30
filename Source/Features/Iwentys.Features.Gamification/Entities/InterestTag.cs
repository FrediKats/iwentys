using System.Collections.Generic;

namespace Iwentys.Features.Gamification.Entities
{
    public class InterestTag
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<StudentInterestTag> UserInterestTags { get; set; }
    }
}