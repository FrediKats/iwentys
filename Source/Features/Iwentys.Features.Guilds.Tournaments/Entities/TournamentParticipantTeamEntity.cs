using System;
using Iwentys.Features.Guilds.Entities;

namespace Iwentys.Features.Guilds.Tournaments.Entities
{
    public class TournamentParticipantTeamEntity
    {
        public int TournamentId { get; set; }
        public virtual TournamentEntity Tournament { get; set; }

        public int GuildId { get; set; }
        public virtual GuildEntity Guild { get; set; }

        public DateTime RegistrationTime { get; set; }
    }
}