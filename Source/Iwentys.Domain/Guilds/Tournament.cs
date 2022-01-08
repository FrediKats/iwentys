using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.Guilds
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
        public virtual IwentysUser Author { get; init; }
        public virtual ICollection<TournamentParticipantTeam> Teams { get; init; }

        public bool IsActive => DateTime.UtcNow < EndTime && !FinishedManually;

        public Tournament()
        {
            Teams = new List<TournamentParticipantTeam>();
        }

        public static Tournament Create(SystemAdminUser author, CreateTournamentArguments arguments, TournamentType type)
        {
            return new Tournament
            {
                Name = arguments.Name,
                Description = arguments.Description,
                StartTime = arguments.StartTime,
                EndTime = arguments.EndTime,
                Type = type,
                AuthorId = author.User.Id,
                FinishedManually = false
            };
        }

        public TournamentParticipantTeam RegisterTeam(IwentysUser guildMentor, Guild guild)
        {
            //TODO: check guild for null

            guildMentor.EnsureIsGuildMentor(guild);
            var team = new TournamentParticipantTeam
            {
                TournamentId = Id,
                GuildId = guild.Id,
                RegistrationTime = DateTime.UtcNow,
                Members = guild.Members.Select(m => new TournamentTeamMember {MemberId = m.MemberId}).ToList()
            };
            Teams.Add(team);
            return team;
        }

        public void FinishManually(IwentysUser user)
        {
            if (user.Id != AuthorId)
                user.EnsureIsAdmin();

            FinishedManually = true;
        }
    }
}