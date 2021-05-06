using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Enums;
using Iwentys.Domain.Study;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.DataAccess.Seeding.EntityGenerators
{
    public class TournamentGenerator : IEntityGenerator
    {
        public TournamentGenerator(List<Student> students, List<Guild> guilds, List<GuildMember> members)
        {
            var faker = new Faker();
            SystemAdminUser admin = students.First(s => s.IsAdmin).EnsureIsAdmin();

            var tournamentEntity = new Tournament
            {
                AuthorId = admin.User.Id,
                Id = 1,
                Name = "Test tournament",
                StartTime = DateTime.UtcNow.AddMonths(-6),
                EndTime = DateTime.UtcNow.AddHours(1),
                Description = faker.Lorem.Paragraph(),
                Type = TournamentType.CodeMarathon
            };

            var marathonTournamentEntity = new CodeMarathonTournament
            {
                Id = 1,
                ActivityType = CodeMarathonAllowedActivityType.All,
                MembersType = CodeMarathonAllowedMembersType.All
            };

            CodeMarathonTournaments = new List<CodeMarathonTournament> {marathonTournamentEntity};
            Tournaments = new List<Tournament> {tournamentEntity};
            TournamentParticipantTeams = new List<TournamentParticipantTeam>();
            TournamentTeamMember = new List<TournamentTeamMember>();

            foreach (Guild guild in guilds)
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
                    .Select(m => new TournamentTeamMember {MemberId = m.MemberId, TeamId = team.Id}));
            }
        }

        public List<Tournament> Tournaments { get; set; }
        public List<CodeMarathonTournament> CodeMarathonTournaments { get; set; }
        public List<TournamentParticipantTeam> TournamentParticipantTeams { get; set; }
        public List<TournamentTeamMember> TournamentTeamMember { get; set; }

        public void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tournament>().HasData(Tournaments);
            modelBuilder.Entity<CodeMarathonTournament>().HasData(CodeMarathonTournaments);
            modelBuilder.Entity<TournamentParticipantTeam>().HasData(TournamentParticipantTeams);
            modelBuilder.Entity<TournamentTeamMember>().HasData(TournamentTeamMember);
        }
    }
}