using System;
using System.Collections.Generic;
using Iwentys.Features.Guilds.Entities;

namespace Iwentys.Features.Guilds.Tournaments.Entities
{
    public class TournamentParticipantTeamEntity
    {
        public int Id { get; set; }
        
        public int TournamentId { get; set; }
        public virtual TournamentEntity Tournament { get; set; }

        public int GuildId { get; set; }
        public virtual GuildEntity Guild { get; set; }

        public DateTime RegistrationTime { get; set; }
        public virtual ICollection<TournamentTeamMemberEntity> Members { get; set; }
    }
}