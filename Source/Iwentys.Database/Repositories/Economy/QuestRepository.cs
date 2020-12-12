using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Database.Context;
using Iwentys.Features.Quests.Entities;
using Iwentys.Features.Quests.Models;
using Iwentys.Features.Quests.Repositories;
using Iwentys.Features.Students.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Economy
{
    public class QuestRepository : IQuestRepository
    {
        private readonly IwentysDbContext _dbContext;

        public QuestRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbSet<QuestEntity> Quests => _dbContext.Quests;

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
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

        public async Task SendResponseAsync(QuestResponseEntity questResponse)
        {
            _dbContext.QuestResponses.Add(questResponse);
            await _dbContext.SaveChangesAsync();
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