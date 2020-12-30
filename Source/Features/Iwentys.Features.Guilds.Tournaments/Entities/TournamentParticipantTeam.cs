using System;
using System.Collections.Generic;
using Iwentys.Features.Guilds.Entities;

namespace Iwentys.Features.Guilds.Tournaments.Entities
{
    public class TournamentParticipantTeam
    {
        public int Id { get; set; }
        
        public int TournamentId { get; set; }
        public virtual Tournament Tournament { get; set; }

        public int GuildId { get; set; }
        public virtual Guild Guild { get; set; }

        public DateTime RegistrationTime { get; set; }
        public virtual ICollection<TournamentTeamMember> Members { get; set; }
    }
}