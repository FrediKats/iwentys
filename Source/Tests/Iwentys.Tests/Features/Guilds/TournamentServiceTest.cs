using System.Linq;
using System.Threading.Tasks;
using Iwentys.Database.Seeding.FakerEntities.Guilds;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Guilds.Models;
using Iwentys.Features.Guilds.Tournaments.Enums;
using Iwentys.Features.Guilds.Tournaments.Models;
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
            TestCaseContext testCase = TestCaseContext
                .Case();
            AuthorizedUser admin = testCase.AccountManagementTestCaseContext.WithUser(true);


            TournamentInfoResponse tournament = await testCase.TournamentService.CreateCodeMarathon(admin, TournamentFaker.Instance.NewCodeMarathon());

            Assert.AreEqual(TournamentType.CodeMarathon, tournament.Type);
        }

        [Test]
        public async Task RegisterTournamentTeam_TeamCreated()
        {
            TestCaseContext testCase = TestCaseContext
                .Case();
            AuthorizedUser admin = testCase.AccountManagementTestCaseContext.WithUser(true);
            GuildProfileDto guild = testCase.GuildTestCaseContext.WithGuild(admin);

            TournamentInfoResponse tournament = await testCase.TournamentService.CreateCodeMarathon(admin, TournamentFaker.Instance.NewCodeMarathon());
            await testCase.TournamentService.RegisterToTournament(admin, tournament.Id);
            tournament = await testCase.TournamentService.Get(tournament.Id);

            Assert.That(tournament.Teams.Any(t => t.TeamName == guild.Title));
        }

        [Test]
        public async Task RegisterTournamentTeam_ShouldBeInMembers()
        {
            TestCaseContext testCase = TestCaseContext
                .Case();
            AuthorizedUser admin = testCase.AccountManagementTestCaseContext.WithUser(true);
            GuildProfileDto guild = testCase.GuildTestCaseContext.WithGuild(admin);

            TournamentInfoResponse tournament = await testCase.TournamentService.CreateCodeMarathon(admin, TournamentFaker.Instance.NewCodeMarathon());

            await testCase.TournamentService.RegisterToTournament(admin, tournament.Id);
            tournament = await testCase.TournamentService.Get(tournament.Id);

            TournamentTeamInfoDto team = tournament.Teams.First(t => t.TeamName == guild.Title);
            Assert.That(team.MemberIds.Contains(admin.Id));
        }
    }
}