using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Database.Context;
using Iwentys.Models.Entities;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable;
using Iwentys.Models.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories
{
    public class QuestRepository : IGenericRepository<QuestEntity, int>
    {
        private readonly IwentysDbContext _dbContext;

        public QuestRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<QuestEntity> Read()
        {
            return _dbContext.Quests
                .Include(q => q.Author)
                .Include(r => r.Responses);
        }

        public Task<QuestEntity> ReadByIdAsync(int key)
        {
            return Read().FirstOrDefaultAsync(q => q.Id == key);
        }

        public async Task<QuestEntity> UpdateAsync(QuestEntity entity)
        {
            EntityEntry<QuestEntity> createdEntity = _dbContext.Quests.Update(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public Task<int> DeleteAsync(int key)
        {
            return _dbContext.Quests.Where(q => q.Id == key).DeleteFromQueryAsync();
        }

        public void SendResponse(QuestEntity questEntity, int userId)
        {
            _dbContext.QuestResponses.Add(QuestResponseEntity.New(questEntity.Id, userId));
            _dbContext.SaveChanges();
        }

        public QuestEntity SetCompleted(QuestEntity questEntity, int studentId)
        {
            if (questEntity.State != QuestState.Active || questEntity.IsOutdated)
                throw new InnerLogicException("Quest is not active");

            questEntity.State = QuestState.Completed;
            questEntity = _dbContext.Quests.Update(questEntity).Entity;

            QuestResponseEntity responseEntity = _dbContext.QuestResponses.Single(qr => qr.QuestId == questEntity.Id && qr.StudentId == studentId);
            List<QuestResponseEntity> responsesToDelete = questEntity.Responses.Where(qr => qr.StudentId != responseEntity.StudentId).ToList();
            _dbContext.QuestResponses.RemoveRange(responsesToDelete);

            StudentEntity student = _dbContext.Students.Find(studentId);
            student.BarsPoints += questEntity.Price;
            _dbContext.Students.Update(student);

            _dbContext.SaveChanges();

            return questEntity;
        }

        public async Task<QuestEntity> CreateAsync(StudentEntity student, CreateQuestRequest createQuest)
        {
            //TODO: add transaction
            if (student.BarsPoints < createQuest.Price)
                throw InnerLogicException.NotEnoughBarsPoints();

            student.BarsPoints -= createQuest.Price;
            var quest = QuestEntity.New(createQuest.Title, createQuest.Description, createQuest.Price, createQuest.Deadline, student);
            EntityEntry<QuestEntity> entity = await _dbContext.Quests.AddAsync(quest);
            await _dbContext.SaveChangesAsync();
            return entity.Entity;
        }
    }
}