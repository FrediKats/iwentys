using System;
using Iwentys.Features.Guilds.Tournaments.Enums;

namespace Iwentys.Features.Guilds.Tournaments.Models
{
    public class CreateCodeMarathonTournamentArguments : CreateTournamentArguments
    {
        public CodeMarathonAllowedMembersType MembersType { get; set; }
        public CodeMarathonAllowedActivityType ActivityType { get; set; }
    }
}