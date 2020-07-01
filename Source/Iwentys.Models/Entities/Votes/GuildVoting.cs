namespace Iwentys.Models.Entities.Votes
{
    public class GuildVoting
    {
        public GuildProfile GuildProfile { get; set; }
        public int GuildProfileId { get; set; }

        public Voting Voting { get; set; }
        public int VotingId { get; set; }
    }
}