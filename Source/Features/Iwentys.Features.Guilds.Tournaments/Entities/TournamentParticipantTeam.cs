using System;
using System.Collections.Generic;
using Iwentys.Features.Guilds.Entities;

namespace Iwentys.Features.Guilds.Tournaments.Entities
{
    public class TournamentParticipantTeam
    {
        public int Id { get; init; }

        public int TournamentId { get; init; }
        public virtual Tournament Tournament { get; init; }

        public int GuildId { get; init; }
        public virtual Guild Guild { get; init; }

        public DateTime RegistrationTime { get; init; }
        public virtual ICollection<TournamentTeamMember> Members { get; init; }
    }
}