using Iwentys.Models.Entities.Guilds;

namespace Iwentys.Models.Entities.Votes
{
    public class GuildVoting
    {
        public Guild Guild { get; set; }
        public int GuildProfileId { get; set; }

        public Voting Voting { get; set; }
        public int VotingId { get; set; }
    }
}