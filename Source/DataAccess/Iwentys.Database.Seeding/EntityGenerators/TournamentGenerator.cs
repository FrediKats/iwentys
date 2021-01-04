using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Tournaments.Entities;
using Iwentys.Features.Guilds.Tournaments.Enums;
using Iwentys.Features.Study.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class TournamentGenerator : IEntityGenerator
    {
        public List<Tournament> Tournaments { get; set; }
        public List<CodeMarathonTournament> CodeMarathonTournaments { get; set; }
        public List<TournamentParticipantTeam> TournamentParticipantTeams { get; set; }
        public List<TournamentTeamMember> TournamentTeamMember { get; set; }

        public TournamentGenerator(List<Student> students, List<Guild> guilds, List<GuildMember> members)
        {
            var faker = new Faker();
            var admin = students.First(s => s.IsAdmin).EnsureIsAdmin();
            
            var tournamentEntity = new Tournament()
            {
                AuthorId = admin.User.Id,
                Id = 1,
                Name = "Test tournament",
                StartTime = DateTime.UtcNow.AddMonths(-6),
                EndTime = DateTime.UtcNow.AddHours(1),
                Description = faker.Lorem.Paragraph(),
                Type = TournamentType.CodeMarathon
            };
            var marathonTournamentEntity = new CodeMarathonTournament()
            {
                Id = 1,
                ActivityType = CodeMarathonAllowedActivityType.All,
                MembersType = CodeMarathonAllowedMembersType.All,
            };

            CodeMarathonTournaments = new List<CodeMarathonTournament> {marathonTournamentEntity};
            Tournaments = new List<Tournament> {tournamentEntity};
            TournamentParticipantTeams = new List<TournamentParticipantTeam>();
            TournamentTeamMember = new List<TournamentTeamMember>();

            foreach (var guild in guilds)
            {
                var team = new TournamentParticipantTeam
                {
                    Id = faker.IndexVariable++ + 1,
                    GuildId = guild.Id,
                    TournamentId = marathonTournamentEntity.Id,
                    RegistrationTime = DateTime.UtcNow
                };

                TournamentParticipantTeams.Add(team);

                TournamentTeamMember.AddRange(members
                    .Where(m => m.GuildId == guild.Id)
                    .Select(m => new TournamentTeamMember { MemberId = m.MemberId, TeamId = team.Id }));
            }
        }

        public void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tournament>().HasData(Tournaments);
            modelBuilder.Entity<CodeMarathonTournament>().HasData(CodeMarathonTournaments);
            modelBuilder.Entity<TournamentParticipantTeam>().HasData(TournamentParticipantTeams);
            modelBuilder.Entity<TournamentTeamMember>().HasData(TournamentTeamMember);
        }
    }
}