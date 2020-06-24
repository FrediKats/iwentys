using System.Linq;
using Iwentys.Database.Entities;
using Iwentys.Database.Transferable.Guilds;
using Iwentys.Tests.Tools;
using NUnit.Framework;

namespace Iwentys.Tests.Core.Services
{
    [TestFixture]
    public class GuildServiceTest
    {
        [Test]
        public void CreateGuild_ShouldReturnCreatorAsMember()
        {
            TestCaseContext
                .Case()
                .WithNewUser(out UserProfile user)
                .WithGuild(user, out GuildProfileDto guild);

            Assert.IsTrue(guild.Members.Any(m => m.Id == user.Id));
        }
    }
}