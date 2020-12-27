using System.ComponentModel.DataAnnotations.Schema;
using Iwentys.Features.Guilds.Tournaments.Enums;
using Iwentys.Features.Guilds.Tournaments.Models;
using Iwentys.Features.Students.Domain;

namespace Iwentys.Features.Guilds.Tournaments.Entities
{
    public class CodeMarathonTournamentEntity
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Id))]
        public virtual TournamentEntity Tournament { get; set; }
        
        public CodeMarathonAllowedMembersType MembersType { get; set; }
        public CodeMarathonAllowedActivityType ActivityType { get; set; }

        public static CodeMarathonTournamentEntity Create(SystemAdminUser author, CreateCodeMarathonTournamentArguments arguments)
        {
            return new CodeMarathonTournamentEntity
            {
                Tournament = TournamentEntity.Create(author, arguments, TournamentType.CodeMarathon),
                MembersType = arguments.MembersType,
                ActivityType = arguments.ActivityType
            };
        }
    }
}