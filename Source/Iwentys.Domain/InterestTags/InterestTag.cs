﻿using System.Collections.Generic;

namespace Iwentys.Domain.InterestTags
{
    public class InterestTag
    {
        public int Id { get; init; }
        public string Title { get; init; }

        public virtual ICollection<UserInterestTag> UserInterestTags { get; init; }
    }
}