using System.Linq;
using System.Threading.Tasks;
using Iwentys.Database.Context;
using Iwentys.Features.Economy.Entities;
using Iwentys.Features.Economy.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Economy
{
    public class BarsPointTransactionRepository : IBarsPointTransactionRepository
    {
        private readonly IwentysDbContext _dbContext;

        public BarsPointTransactionRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BarsPointTransactionEntity> CreateAsync(BarsPointTransactionEntity entity)
        {
            EntityEntry<BarsPointTransactionEntity> createdEntity = await _dbContext.BarsPointTransactionLogs.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public IQueryable<BarsPointTransactionEntity> Read()
        {
            return _dbContext.BarsPointTransactionLogs;
        }

        public Task<BarsPointTransactionEntity> ReadByIdAsync(int key)
        {
            return _dbContext.BarsPointTransactionLogs.FirstOrDefaultAsync(v => v.Id == key);
        }

        public async Task<BarsPointTransactionEntity> UpdateAsync(BarsPointTransactionEntity entity)
        {
            EntityEntry<BarsPointTransactionEntity> createdEntity = _dbContext.BarsPointTransactionLogs.Update(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public Task<int> DeleteAsync(int key)
        {
            return _dbContext.BarsPointTransactionLogs.Where(e => e.Id == key).DeleteFromQueryAsync();
        }
    }
}