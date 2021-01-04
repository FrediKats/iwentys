using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.AccountManagement.Models;
using Iwentys.Features.Quests.Models;
using Iwentys.Features.Study.Models.Students;
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
            TestCaseContext testCase = TestCaseContext
                .Case();
            AuthorizedUser user = testCase.AccountManagementTestCaseContext.WithUser();

            testCase
                .WithQuest(user, 50, out QuestInfoDto quest);

            List<QuestInfoDto> quests = await testCase.QuestService.GetActive();

            Assert.IsTrue(quests.Any(q => q.Id == quest.Id));
        }

        [Test]
        public async Task CreateQuest_ReturnAsCreatedByUser()
        {
            TestCaseContext testCase = TestCaseContext
                .Case();
            AuthorizedUser user = testCase.AccountManagementTestCaseContext.WithUser();

            testCase
                .WithQuest(user, 50, out QuestInfoDto quest);

            List<QuestInfoDto> quests = await testCase.QuestService.GetCreatedByUser(user);
            
            Assert.IsTrue(quests.Any(q => q.Id == quest.Id));
        }

        [Test]
        public async Task CompleteQuest()
        {
            TestCaseContext testCase = TestCaseContext
                .Case();
            AuthorizedUser questCreator = testCase.AccountManagementTestCaseContext.WithUser();
            AuthorizedUser questExecutor = testCase.AccountManagementTestCaseContext.WithUser();

            IwentysUserInfoDto questExecuteAccount = await testCase.IwentysUserService.Get(questExecutor.Id);
            int executorPointsCount = questExecuteAccount.BarsPoints;

            testCase.WithQuest(questCreator, 50, out QuestInfoDto quest);
            await testCase.QuestService.SendResponse(questExecutor, quest.Id);
            await testCase.QuestService.Complete(questCreator, quest.Id, questExecutor.Id);

            questExecuteAccount = await testCase.IwentysUserService.Get(questExecutor.Id);
            Assert.AreEqual(executorPointsCount + quest.Price, questExecuteAccount.BarsPoints);
        }

        [Test]
        public async Task RevokeQuest_EnsureCorrectPointsCount()
        {
            TestCaseContext testCase = TestCaseContext
                .Case();
            AuthorizedUser questCreator = testCase.AccountManagementTestCaseContext.WithUser();

            var questCreatorAccount = await testCase.IwentysUserService.Get(questCreator.Id);
            int pointsCountBefore = questCreatorAccount.BarsPoints;

            testCase.WithQuest(questCreator, 50, out QuestInfoDto quest);
            await testCase.QuestService.Revoke(questCreator, quest.Id);

            questCreatorAccount = await testCase.IwentysUserService.Get(questCreator.Id);
            Assert.AreEqual(pointsCountBefore, questCreatorAccount.BarsPoints);
        }
    }
}