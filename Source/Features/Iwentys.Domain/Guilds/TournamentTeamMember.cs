using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.Guilds
{
    public class TournamentTeamMember
    {
        public int TeamId { get; init; }
        public virtual TournamentParticipantTeam Team { get; init; }

        public int MemberId { get; init; }
        public virtual IwentysUser Member { get; init; }

        public int Points { get; set; }
    }
}