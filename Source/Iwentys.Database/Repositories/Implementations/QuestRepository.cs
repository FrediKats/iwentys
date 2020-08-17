using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
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

        public void AcceptQuest(Quest quest, int userId)
        {
            _dbContext.QuestResponses.Add(QuestResponseEntity.New(quest.Id, userId));
            _dbContext.SaveChanges();
        }
    }
}