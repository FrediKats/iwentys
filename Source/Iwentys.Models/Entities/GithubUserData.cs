using System;
using System.Collections.Generic;
using System.Text;
using Iwentys.Models.Types.Github;

namespace Iwentys.Models.Entities
{
    public class GithubUserData
    {
        public Student Student { get; set; }
        public int StudentId { get; set; }

        public int Id { get; set; }
        public GithubUser GithubUser { get; set; }
        public ContributionFullInfo ContributionFullInfo { get; set; }
    }
}
