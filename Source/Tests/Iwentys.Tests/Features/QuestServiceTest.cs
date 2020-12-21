using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.Quests.Models;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
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
                .WithNewStudent(out AuthorizedUser user);

            StudentEntity student = await test.UnitOfWork.GetRepository<StudentEntity>().GetByIdAsync(user.Id);
            student.BarsPoints = 100;
            test.UnitOfWork.GetRepository<StudentEntity>().Update(student);
            await test.UnitOfWork.CommitAsync();

            test.WithQuest(user, 50, out QuestInfoDto quest);

            List<QuestInfoDto> quests = await test.QuestService.GetActiveAsync();

            Assert.IsTrue(quests.Any(q => q.Id == quest.Id));
        }

        [Test]
        public async Task CreateQuest_ReturnAsCreatedByUser()
        {
            TestCaseContext test = TestCaseContext.Case()
                .WithNewStudent(out AuthorizedUser user);

            StudentEntity student = await test.UnitOfWork.GetRepository<StudentEntity>().GetByIdAsync(user.Id);
            student.BarsPoints = 100;
            test.UnitOfWork.GetRepository<StudentEntity>().Update(student);
            await test.UnitOfWork.CommitAsync();

            test.WithQuest(user, 50, out QuestInfoDto quest);

            List<QuestInfoDto> quests = await test.QuestService.GetCreatedByUserAsync(user);
            
            Assert.IsTrue(quests.Any(q => q.Id == quest.Id));
        }

        [Test]
        public async Task CompleteQuest()
        {
            TestCaseContext test = TestCaseContext.Case()
                .WithNewStudent(out AuthorizedUser questCreator)
                .WithNewStudent(out AuthorizedUser questExecute);

            StudentEntity questExecuteAccount = await test.UnitOfWork.GetRepository<StudentEntity>().GetByIdAsync(questExecute.Id);

            int executorPointsCount = questExecuteAccount.BarsPoints; 
            
            test.WithQuest(questCreator, 50, out QuestInfoDto quest);
            await test.QuestService.SendResponseAsync(questExecute, quest.Id);
            await test.QuestService.CompleteAsync(questCreator, quest.Id, questExecute.Id);

            questExecuteAccount = await test.UnitOfWork.GetRepository<StudentEntity>().GetByIdAsync(questExecute.Id);

            Assert.AreEqual(executorPointsCount + quest.Price, questExecuteAccount.BarsPoints);
        }
    }
}