using System.Collections.Generic;
using Iwentys.Models.Entities.Guilds;

namespace Iwentys.Models.Entities.Votes
{
    public class GuildLeaderVoting
    {
        public GuildEntity Guild { get; set; }
        public int GuildProfileId { get; set; }

        public Voting Voting { get; set; }
        public int VotingId { get; set; }

        public List<GuildLeaderVotingCandidates> Candidates { get; set; }
    }
}