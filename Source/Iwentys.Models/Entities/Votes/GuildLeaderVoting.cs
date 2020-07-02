using System.Collections.Generic;

namespace Iwentys.Models.Entities.Votes
{
    public class GuildLeaderVoting
    {
        public Guild Guild { get; set; }
        public int GuildProfileId { get; set; }

        public Voting Voting { get; set; }
        public int VotingId { get; set; }

        public List<GuildLeaderVotingCandidates> Candidates { get; set; }
    }
}