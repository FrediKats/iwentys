using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Iwentys.Features.Guilds.Tournaments.Entities;
using Iwentys.Features.Guilds.Tournaments.Enums;
using Iwentys.Features.Guilds.Tournaments.Models;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Enums;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class TournamentGenerator
    {
        public List<TournamentEntity> Tournaments { get; set; }
        public List<CodeMarathonTournamentEntity> CodeMarathonTournaments { get; set; }
        public List<TournamentParticipantTeamEntity> TournamentParticipantTeams { get; set; }
        public List<TournamentTeamMemberEntity> TournamentTeamMember { get; set; }

        public TournamentGenerator(List<StudentEntity> students)
        {
            var faker = new Faker();
            var admin = students.First(s => s.Role == StudentRole.Admin).EnsureIsAdmin();
            var createCodeMarathonTournamentArguments = new CreateCodeMarathonTournamentArguments()
            {
                ActivityType = CodeMarathonAllowedActivityType.All,
                MembersType = CodeMarathonAllowedMembersType.All,
                StartTime = DateTime.UtcNow.AddHours(-1),
                EndTime = DateTime.UtcNow.AddHours(1),
                Name = "Test tournament"
            };
            var tournamentEntity = new TournamentEntity()
            {
                AuthorId = admin.Student.Id,
                Id = 1,
                Name = "Test tournament",
                StartTime = DateTime.UtcNow.AddHours(-1),
                EndTime = DateTime.UtcNow.AddHours(1),
                Description = faker.Lorem.Paragraph(),
                Type = TournamentType.CodeMarathon
            };
            var marathonTournamentEntity = new CodeMarathonTournamentEntity()
            {
                Id = 1,
                ActivityType = CodeMarathonAllowedActivityType.All,
                MembersType = CodeMarathonAllowedMembersType.All,
            };

            var codeMarathonTournamentEntity = CodeMarathonTournamentEntity.Create(admin, createCodeMarathonTournamentArguments);
            CodeMarathonTournaments = new List<CodeMarathonTournamentEntity> {marathonTournamentEntity};
            Tournaments = new List<TournamentEntity> {tournamentEntity};
            TournamentParticipantTeams = new List<TournamentParticipantTeamEntity>();
            TournamentTeamMember = new List<TournamentTeamMemberEntity>();
        }
    }
}