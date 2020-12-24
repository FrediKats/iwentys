using System.Collections.Generic;

namespace Iwentys.Features.Gamification.Entities
{
    public class InterestTagEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<StudentInterestTagEntity> UserInterestTags { get; set; }
    }
}