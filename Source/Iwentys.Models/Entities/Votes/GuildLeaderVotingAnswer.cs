namespace Iwentys.Models.Entities.Votes
{
    public class GuildLeaderVotingAnswer
    {
        public Voting Voting { get; set; }
        public int VotingId { get; set; }

        public UserProfile Candidate { get; set; }
        public int CandidateId { get; set; }

        public UserProfile User { get; set; }
        public int UserId { get; set; }
    }
}