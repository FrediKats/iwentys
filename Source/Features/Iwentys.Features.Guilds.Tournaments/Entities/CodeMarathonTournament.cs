using System.ComponentModel.DataAnnotations.Schema;
using Iwentys.Features.Guilds.Tournaments.Enums;
using Iwentys.Features.Guilds.Tournaments.Models;
using Iwentys.Features.Students.Domain;

namespace Iwentys.Features.Guilds.Tournaments.Entities
{
    public class CodeMarathonTournament
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Id))]
        public virtual Tournament Tournament { get; set; }
        
        public CodeMarathonAllowedMembersType MembersType { get; set; }
        public CodeMarathonAllowedActivityType ActivityType { get; set; }

        public static CodeMarathonTournament Create(SystemAdminUser author, CreateCodeMarathonTournamentArguments arguments)
        {
            return new CodeMarathonTournament
            {
                Tournament = Tournament.Create(author, arguments, TournamentType.CodeMarathon),
                MembersType = arguments.MembersType,
                ActivityType = arguments.ActivityType
            };
        }
    }
}