using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.AccountManagement.Models;
using Iwentys.Features.Quests.Models;
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
            TestCaseContext testCase = TestCaseContext.Case();
            AuthorizedUser user = testCase.AccountManagementTestCaseContext.WithUser();
            QuestInfoDto quest = testCase.QuestTestCaseContext.WithQuest(user, 50);

            List<QuestInfoDto> quests = await testCase.QuestService.GetActive();

            Assert.IsTrue(quests.Any(q => q.Id == quest.Id));
        }

        [Test]
        public async Task CreateQuest_ReturnAsCreatedByUser()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            AuthorizedUser user = testCase.AccountManagementTestCaseContext.WithUser();
            QuestInfoDto quest = testCase.QuestTestCaseContext.WithQuest(user, 50);

            List<QuestInfoDto> quests = await testCase.QuestService.GetCreatedByUser(user);

            Assert.IsTrue(quests.Any(q => q.Id == quest.Id));
        }

        [Test]
        public async Task CompleteQuest()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            AuthorizedUser questCreator = testCase.AccountManagementTestCaseContext.WithUser();
            AuthorizedUser questExecutor = testCase.AccountManagementTestCaseContext.WithUser();
            QuestInfoDto quest = testCase.QuestTestCaseContext.WithQuest(questCreator, 50);

            IwentysUserInfoDto questExecuteAccount = await testCase.IwentysUserService.Get(questExecutor.Id);
            var executorPointsCount = questExecuteAccount.BarsPoints;

            await testCase.QuestService.SendResponse(questExecutor, quest.Id, new QuestResponseCreateArguments());
            await testCase.QuestService.Complete(questCreator, quest.Id, new QuestCompleteArguments() {UserId = questExecutor.Id , Mark = 5});

            questExecuteAccount = await testCase.IwentysUserService.Get(questExecutor.Id);
            Assert.AreEqual(executorPointsCount + quest.Price, questExecuteAccount.BarsPoints);
        }

        [Test]
        public async Task RevokeQuest_EnsureCorrectPointsCount()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            AuthorizedUser questCreator = testCase.AccountManagementTestCaseContext.WithUser();
            IwentysUserInfoDto questCreatorAccount = await testCase.IwentysUserService.Get(questCreator.Id);
            var pointsCountBefore = questCreatorAccount.BarsPoints;
            QuestInfoDto quest = testCase.QuestTestCaseContext.WithQuest(questCreator, 50);

            await testCase.QuestService.Revoke(questCreator, quest.Id);

            questCreatorAccount = await testCase.IwentysUserService.Get(questCreator.Id);
            Assert.AreEqual(pointsCountBefore, questCreatorAccount.BarsPoints);
        }
    }
}