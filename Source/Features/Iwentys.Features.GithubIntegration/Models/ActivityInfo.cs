﻿using System.Collections.Generic;

namespace Iwentys.Features.GithubIntegration.Models
{
    public class ActivityInfo
    {
        public int Id { get; set; }
        public List<YearActivityInfo> Years { get; set; }
        public List<ContributionsInfo> Contributions { get; set; }
    }
}