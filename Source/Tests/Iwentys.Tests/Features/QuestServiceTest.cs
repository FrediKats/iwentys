using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.Quests.Models;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Iwentys.Tests.Tools;
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
                .WithNewStudent(out AuthorizedUser user);

            StudentEntity student = await test.StudentRepository.ReadByIdAsync(user.Id);
            student.BarsPoints = 100;
            await test.StudentRepository.UpdateAsync(student);

            test.WithQuest(user, 50, out QuestInfoResponse quest);

            List<QuestInfoResponse> quests = await test.QuestService.GetActiveAsync();

            Assert.IsTrue(quests.Any(q => q.Id == quest.Id));
        }

        [Test]
        public async Task CreateQuest_ReturnAsCreatedByUser()
        {
            TestCaseContext test = TestCaseContext.Case()
                .WithNewStudent(out AuthorizedUser user);

            StudentEntity student = await test.StudentRepository.ReadByIdAsync(user.Id);
            student.BarsPoints = 100;
            await test.StudentRepository.UpdateAsync(student);

            test.WithQuest(user, 50, out QuestInfoResponse quest);

            List<QuestInfoResponse> quests = await test.QuestService.GetCreatedByUserAsync(user);
            
            Assert.IsTrue(quests.Any(q => q.Id == quest.Id));
        }

        [Test]
        public async Task CompleteQuest()
        {
            TestCaseContext test = TestCaseContext.Case()
                .WithNewStudent(out AuthorizedUser questCreator)
                .WithNewStudent(out AuthorizedUser questExecute);

            StudentEntity questCreatorAccount = await test.StudentRepository.ReadByIdAsync(questCreator.Id);
            StudentEntity questExecuteAccount = await test.StudentRepository.ReadByIdAsync(questExecute.Id);

            //TODO: remove opportunity for such updating. Need transaction from system
            questCreatorAccount.BarsPoints = 100;
            await test.StudentRepository.UpdateAsync(questCreatorAccount);
            int executorPointsCount = questExecuteAccount.BarsPoints; 
            
            test.WithQuest(questCreator, 50, out QuestInfoResponse quest);
            await test.QuestService.SendResponseAsync(questExecute, quest.Id);
            await test.QuestService.CompleteAsync(questCreator, quest.Id, questExecute.Id);

            questExecuteAccount = await test.StudentRepository.ReadByIdAsync(questExecute.Id);

            Assert.AreEqual(executorPointsCount + quest.Price, questExecuteAccount.BarsPoints);
        }
    }
}