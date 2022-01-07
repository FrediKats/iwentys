using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.VotingSystem
{
    public class GuildLeaderVotingCandidates
    {
        public int VotingId { get; init; }
        public virtual Voting Voting { get; init; }

        public int CandidateId { get; init; }
        public virtual IwentysUser Candidate { get; init; }
    }
}