using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Models.Entities;
using Iwentys.Models.Transferable.Gamification;
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

            StudentEntity student = user.GetProfile(test.StudentRepository).Result;
            student.BarsPoints = 100;
            test.StudentRepository.Update(student);

            test.WithQuest(user, 50, out QuestInfoResponse quest);

            List<QuestInfoResponse> quests = test.QuestService.GetActive();

            Assert.IsTrue(quests.Any(q => q.Id == quest.Id));
        }
    }
}