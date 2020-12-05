using System;
using System.Collections.Generic;

namespace Iwentys.Features.Voting.Models
{
    public class VotingCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime DueTo { get; set; }
        public List<string> Variants { get; set; }
    }
}