using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Voting.Entities
{
    public class GuildLeaderVotingCandidates
    {
        public Voting Voting { get; set; }
        public int VotingId { get; set; }

        public Student Candidate { get; set; }
        public int CandidateId { get; set; }
    }
}