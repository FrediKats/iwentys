using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Tournaments.Entities
{
    public class TournamentTeamMemberEntity
    {
        public int TeamId { get; set; }
        public virtual TournamentParticipantTeamEntity Team { get; set; }

        public int MemberId { get; set; }
        public virtual StudentEntity Member { get; set; }
    }
}