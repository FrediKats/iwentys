using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Models.Transferable.Gamification;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Tests.Tools;
using NUnit.Framework;

namespace Iwentys.Tests.Core.Services
{
    [TestFixture]
    public class QuestServiceTest
    {
        [Test]
        public void CreateGuild_ShouldReturnCreatorAsMember()
        {
            TestCaseContext test = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user);

            user.Profile.BarsPoints = 100;

            test.StudentRepository.Update(user.Profile);

            test.WithQuest(user, 50, out QuestInfoDto quest);

            List<QuestInfoDto> quests = test.QuestService.GetActive();

            Assert.IsTrue(quests.Any(q => q.Id == quest.Id));
        }
    }
}