using Iwentys.Database.Seeding.FakerEntities;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Quests;
using Iwentys.Domain.Quests.Dto;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features
{
    [TestFixture]
    public class QuestServiceTest
    {
        [Test]
        public void CreateQuest_ReturnAsActive()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            IwentysUser user = testCase.AccountManagementTestCaseContext.WithIwentysUser();

            var quest = Quest.New(user, QuestFaker.Instance.CreateQuestRequest(50));

            Assert.IsTrue(quest.State == QuestState.Active);
        }

        [Test]
        public void CompleteQuest()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            IwentysUser questCreator = testCase.AccountManagementTestCaseContext.WithIwentysUser();
            IwentysUser questExecutor = testCase.AccountManagementTestCaseContext.WithIwentysUser();
            int executorPointsCount = questExecutor.BarsPoints;

            var quest = Quest.New(questCreator, QuestFaker.Instance.CreateQuestRequest(50));

            quest.CreateResponse(questExecutor, new QuestResponseCreateArguments());
            quest.MakeCompleted(questCreator, questExecutor, new QuestCompleteArguments() { UserId = questExecutor.Id, Mark = 5 });

            //TODO: fix points transaction
            //await _pointTransactionLogService.TransferFromSystem(executor.Id, quest.Price);
            //_achievementProvider.AchieveForStudent(AchievementList.QuestComplete, executor.Id);
            //Assert.AreEqual(executorPointsCount + quest.Price, questExecutor.BarsPoints);
        }

        [Test]
        public void RevokeQuest_EnsureCorrectPointsCount()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            IwentysUser questCreator = testCase.AccountManagementTestCaseContext.WithIwentysUser();
            int pointsCountBefore = questCreator.BarsPoints;

            var quest = Quest.New(questCreator, QuestFaker.Instance.CreateQuestRequest(50));
            quest.Revoke(questCreator);

            Assert.AreEqual(pointsCountBefore, questCreator.BarsPoints);
        }
    }
}