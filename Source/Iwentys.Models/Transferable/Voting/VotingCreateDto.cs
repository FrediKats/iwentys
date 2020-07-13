using System;
using System.Collections.Generic;

namespace Iwentys.Models.Transferable.Voting
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