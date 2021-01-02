using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Quests.Models;
using Iwentys.Features.Students.Models;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features
{
    [TestFixture]
    public class QuestServiceTest
    {
        [Test]
        public async Task CreateQuest_ReturnAsActive()
        {
            TestCaseContext test = TestCaseContext.Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithQuest(user, 50, out QuestInfoDto quest);

            List<QuestInfoDto> quests = await test.QuestService.GetActiveAsync();

            Assert.IsTrue(quests.Any(q => q.Id == quest.Id));
        }

        [Test]
        public async Task CreateQuest_ReturnAsCreatedByUser()
        {
            TestCaseContext test = TestCaseContext.Case()
                .WithNewStudent(out AuthorizedUser user)
                .WithQuest(user, 50, out QuestInfoDto quest);

            List<QuestInfoDto> quests = await test.QuestService.GetCreatedByUserAsync(user);
            
            Assert.IsTrue(quests.Any(q => q.Id == quest.Id));
        }

        [Test]
        public async Task CompleteQuest()
        {
            TestCaseContext test = TestCaseContext.Case()
                .WithNewStudent(out AuthorizedUser questCreator)
                .WithNewStudent(out AuthorizedUser questExecutor);

            StudentInfoDto questExecuteAccount = await test.StudentService.GetAsync(questExecutor.Id);
            int executorPointsCount = questExecuteAccount.BarsPoints; 
            
            test.WithQuest(questCreator, 50, out QuestInfoDto quest);
            await test.QuestService.SendResponseAsync(questExecutor, quest.Id);
            await test.QuestService.CompleteAsync(questCreator, quest.Id, questExecutor.Id);

            questExecuteAccount = await test.StudentService.GetAsync(questExecutor.Id);
            Assert.AreEqual(executorPointsCount + quest.Price, questExecuteAccount.BarsPoints);
        }

        [Test]
        public async Task RevokeQuest_EnsureCorrectPointsCount()
        {
            TestCaseContext test = TestCaseContext.Case()
                .WithNewStudent(out AuthorizedUser questCreator);

            var questCreatorAccount = await test.StudentService.GetAsync(questCreator.Id);
            int pointsCountBefore = questCreatorAccount.BarsPoints;

            test.WithQuest(questCreator, 50, out QuestInfoDto quest);
            await test.QuestService.RevokeAsync(questCreator, quest.Id);

            questCreatorAccount = await test.StudentService.GetAsync(questCreator.Id);
            Assert.AreEqual(pointsCountBefore, questCreatorAccount.BarsPoints);
        }
    }
}