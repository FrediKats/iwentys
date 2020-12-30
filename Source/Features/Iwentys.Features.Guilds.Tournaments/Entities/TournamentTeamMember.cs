using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Tournaments.Entities
{
    public class TournamentTeamMember
    {
        public int TeamId { get; set; }
        public virtual TournamentParticipantTeam Team { get; set; }

        public int MemberId { get; set; }
        public virtual Student Member { get; set; }

        public int Points { get; set; }
    }
} 