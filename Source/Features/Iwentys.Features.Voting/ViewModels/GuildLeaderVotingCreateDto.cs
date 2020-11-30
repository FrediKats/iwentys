using System;
using System.Collections.Generic;

namespace Iwentys.Features.Voting.ViewModels
{
    public class GuildLeaderVotingCreateDto
    {
        public List<int> Candidates { get; set; }
        public DateTime StartTime { get; set; }
    }
}