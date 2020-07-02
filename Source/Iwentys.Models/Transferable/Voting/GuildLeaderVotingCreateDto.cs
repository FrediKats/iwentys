using System;
using System.Collections.Generic;

namespace Iwentys.Models.Transferable.Voting
{
    public class GuildLeaderVotingCreateDto
    {
        public List<int> Candidates { get; set; }
        public DateTime StartTime { get; set; }
    }
}