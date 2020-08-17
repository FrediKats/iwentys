using System.Collections.Generic;
using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Transferable.Gamification;
using Iwentys.Models.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Implementations
{
    public class QuestRepository : IQuestRepository
    {
        private readonly IwentysDbContext _dbContext;

        public QuestRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Quest Create(Quest entity)
        {
            EntityEntry<Quest> createdEntity = _dbContext.Quests.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public IQueryable<Quest> Read()
        {
            return _dbContext.Quests
                .Include(q => q.Author)
                .Include(r => r.Responses);
        }

        public Quest ReadById(int key)
        {
            return Read().FirstOrDefault(q => q.Id == key);
        }

        public Quest Update(Quest entity)
        {
            EntityEntry<Quest> createdEntity = _dbContext.Quests.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(int key)
        {
            _dbContext.Quests.Remove(this.Get(key));
            _dbContext.SaveChanges();
        }

        public void SendResponse(Quest quest, int userId)
        {
            _dbContext.QuestResponses.Add(QuestResponseEntity.New(quest.Id, userId));
            _dbContext.SaveChanges();
        }

        public Quest SetCompleted(Quest quest, int studentId)
        {
            if (quest.State != QuestState.Active || quest.IsOutdated)
                throw new InnerLogicException("Quest is not active");

            quest.State = QuestState.Completed;
            quest = _dbContext.Quests.Update(quest).Entity;

            QuestResponseEntity responseEntity = _dbContext.QuestResponses.Single(qr => qr.QuestId == quest.Id && qr.StudentId == studentId);
            List<QuestResponseEntity> responsesToDelete = quest.Responses.Where(qr => qr.StudentId != responseEntity.StudentId).ToList();
            _dbContext.QuestResponses.RemoveRange(responsesToDelete);

            Student student = _dbContext.Students.Find(studentId);
            student.BarsPoints += quest.Price;
            _dbContext.Students.Update(student);

            _dbContext.SaveChanges();

            return quest;
        }

        public Quest Create(Student student, CreateQuestDto createQuest)
        {
            //TODO: add transaction
            if (student.BarsPoints < createQuest.Price)
                throw InnerLogicException.NotEnoughBarsPoints();

            student.BarsPoints -= createQuest.Price;
            var quest = Quest.New(createQuest.Title, createQuest.Description, createQuest.Price, createQuest.Deadline, student);
            return _dbContext.Quests.Add(quest).Entity;
        }
    }
}