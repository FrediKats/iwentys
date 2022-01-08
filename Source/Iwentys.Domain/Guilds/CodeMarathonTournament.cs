using System.ComponentModel.DataAnnotations.Schema;
using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.Guilds;

public class CodeMarathonTournament
{
    public int Id { get; set; }

    [ForeignKey(nameof(Id))] public virtual Tournament Tournament { get; init; }

    public CodeMarathonAllowedMembersType MembersType { get; init; }
    public CodeMarathonAllowedActivityType ActivityType { get; init; }

    public static CodeMarathonTournament Create(IwentysUser user, CreateCodeMarathonTournamentArguments arguments)
    {
        return new CodeMarathonTournament
        {
            Tournament = Tournament.Create(user.EnsureIsAdmin(), arguments, TournamentType.CodeMarathon),
            MembersType = arguments.MembersType,
            ActivityType = arguments.ActivityType
        };
    }
}