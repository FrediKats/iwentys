using System.Threading.Tasks;
using Iwentys.Features.Guilds.Tournaments.Enums;
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
        public async Task CreateGuild_ShouldReturnCreatorAsMember()
        {
            TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user, StudentRole.Admin)
                .WithCodeMarathon(user, out var tournament);

            Assert.AreEqual(TournamentType.CodeMarathon, tournament.Type);
        }
    }
}