using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.Guilds.Models;
using Iwentys.Features.Guilds.Tournaments.Enums;
using Iwentys.Features.Guilds.Tournaments.Models;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Enums;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features.Guilds
{
    [TestFixture]
    public class TournamentServiceTest
    {
        [Test]
        public async Task CreateCodeMarathonTournament_ShouldHaveCorrectType()
        {
            TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user, StudentRole.Admin)
                .WithCodeMarathon(user, out var tournament);

            Assert.AreEqual(TournamentType.CodeMarathon, tournament.Type);
        }

        [Test]
        public async Task RegisterTournamentTeam_TeamCreated()
        {
            var testCase = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user, StudentRole.Admin)
                .WithCodeMarathon(user, out TournamentInfoResponse tournament)
                .WithGuild(user, out ExtendedGuildProfileWithMemberDataDto guild);

            await testCase.TournamentService.RegisterToTournament(user, tournament.Id);
            tournament = await testCase.TournamentService.GetAsync(tournament.Id);
            
            Assert.That(tournament.Teams.Any(t => t.TeamName == guild.Title));
        }

        [Test]
        public async Task RegisterTournamentTeam_ShouldBeInMembers()
        {
            var testCase = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user, StudentRole.Admin)
                .WithCodeMarathon(user, out TournamentInfoResponse tournament)
                .WithGuild(user, out ExtendedGuildProfileWithMemberDataDto guild);

            await testCase.TournamentService.RegisterToTournament(user, tournament.Id);
            tournament = await testCase.TournamentService.GetAsync(tournament.Id);

            var team = tournament.Teams.First(t => t.TeamName == guild.Title);
            Assert.That(team.MemberIds.Contains(user.Id));
        }
    }
}