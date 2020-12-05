using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Voting.Entities
{
    public class GuildLeaderVotingAnswer
    {
        public Voting Voting { get; set; }
        public int VotingId { get; set; }

        public StudentEntity Candidate { get; set; }
        public int CandidateId { get; set; }

        public StudentEntity Student { get; set; }
        public int StudentId { get; set; }
    }
}