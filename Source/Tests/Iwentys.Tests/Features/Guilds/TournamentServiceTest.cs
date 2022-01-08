using System.Linq;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Infrastructure.DataAccess.Seeding.FakerEntities.Guilds;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features.Guilds
{
    [TestFixture]
    public class TournamentServiceTest
    {
        [Test]
        public void CreateCodeMarathonTournament_ShouldHaveCorrectType()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            IwentysUser admin = testCase.AccountManagementTestCaseContext.WithIwentysUser(true);

            var codeMarathonTournament = CodeMarathonTournament.Create(admin, TournamentFaker.Instance.NewCodeMarathon());

            Assert.AreEqual(TournamentType.CodeMarathon, codeMarathonTournament.Tournament.Type);
        }

        [Test]
        public void RegisterTournamentTeam_TeamCreated()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            IwentysUser admin = testCase.AccountManagementTestCaseContext.WithIwentysUser(true);
            var guild = Guild.Create(admin, null, GuildFaker.Instance.GetGuildCreateArguments());
            var tournament = CodeMarathonTournament.Create(admin, TournamentFaker.Instance.NewCodeMarathon());

            tournament.Tournament.RegisterTeam(admin, guild);

            Assert.That(tournament.Tournament.Teams.Any(t => t.GuildId == guild.Id));
        }
    }
}