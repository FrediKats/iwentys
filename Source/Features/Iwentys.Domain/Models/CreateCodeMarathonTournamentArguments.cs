using Iwentys.Domain.Guilds.Enums;

namespace Iwentys.Domain.Models
{
    public class CreateCodeMarathonTournamentArguments : CreateTournamentArguments
    {
        public CodeMarathonAllowedMembersType MembersType { get; set; }
        public CodeMarathonAllowedActivityType ActivityType { get; set; }
    }
}