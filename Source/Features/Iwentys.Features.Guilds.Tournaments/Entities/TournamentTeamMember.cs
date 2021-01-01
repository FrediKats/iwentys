using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Tournaments.Entities
{
    public class TournamentTeamMember
    {
        public int TeamId { get; init; }
        public virtual TournamentParticipantTeam Team { get; init; }

        public int MemberId { get; init; }
        public virtual Student Member { get; init; }

        public int Points { get; set; }
    }
} 