namespace Iwentys.Models.Entities.Votes
{
    public class GuildTotemVoting
    {
        public GuildProfile GuildProfile { get; set; }
        public int GuildProfileId { get; set; }

        public Voting Voting { get; set; }
        public int VotingId { get; set; }

        public UserProfile TotemCandidate { get; set; }
        public int TotemCandidateId { get; set; }
    }
}