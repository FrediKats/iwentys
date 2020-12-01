using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.Quests.ViewModels;
using Iwentys.Features.StudentFeature;
using Iwentys.Models.Entities;
using Iwentys.Tests.Tools;
using NUnit.Framework;

namespace Iwentys.Tests.Core.Services
{
    [TestFixture]
    public class QuestServiceTest
    {
        [Test]
        public async Task CreateGuild_ShouldReturnCreatorAsMember()
        {
            TestCaseContext test = TestCaseContext
                .Case()
                .WithNewStudent(out AuthorizedUser user);

            StudentEntity student = await user.GetProfile(test.StudentRepository);
            student.BarsPoints = 100;
            await test.StudentRepository.UpdateAsync(student);

            test.WithQuest(user, 50, out QuestInfoResponse quest);

            List<QuestInfoResponse> quests = await test.QuestService.GetActiveAsync();

            Assert.IsTrue(quests.Any(q => q.Id == quest.Id));
        }
    }
}