using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Tournaments.Enums;
using Iwentys.Features.Guilds.Tournaments.Models;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Tournaments.Entities
{
    public class Tournament
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public DateTime StartTime { get; init; }
        public DateTime EndTime { get; init; }
        public TournamentType Type { get; init; }
        public bool FinishedManually { get; private set; }
        
        public int AuthorId { get; init; }
        public virtual Student Author { get; init; }
        public virtual ICollection<TournamentParticipantTeam> Teams { get; init; }

        public bool IsActive => DateTime.UtcNow < EndTime && !FinishedManually;

        public static Tournament Create(SystemAdminUser author, CreateTournamentArguments arguments, TournamentType type)
        {
            return new Tournament
            {
                Name = arguments.Name,
                Description = arguments.Description,
                StartTime = arguments.StartTime,
                EndTime = arguments.EndTime,
                Type = type,
                AuthorId = author.Student.Id,
                FinishedManually = false
            };
        }

        public TournamentParticipantTeam RegisterTeam(Guild guild, List<GuildMember> members)
        {
            return new TournamentParticipantTeam
            {
                TournamentId = Id,
                GuildId = guild.Id,
                RegistrationTime = DateTime.UtcNow,
                Members = members.Select(m => new TournamentTeamMember {MemberId = m.MemberId}).ToList()
            };
        }

        public void FinishManually(Student user)
        {
            if (user.Id != AuthorId)
                user.EnsureIsAdmin();

            FinishedManually = true;
        }
    }
}