namespace Iwentys.Domain.Guilds;

public class CreateCodeMarathonTournamentArguments : CreateTournamentArguments
{
    public CodeMarathonAllowedMembersType MembersType { get; set; }
    public CodeMarathonAllowedActivityType ActivityType { get; set; }
}