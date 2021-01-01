using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Voting.Entities
{
    public class GuildLeaderVotingAnswer
    {
        public int VotingId { get; init; }
        public virtual Voting Voting { get; init; }

        public int CandidateId { get; init; }
        public virtual Student Candidate { get; init; }

        public int StudentId { get; init; }
        public virtual Student Student { get; init; }
    }
}