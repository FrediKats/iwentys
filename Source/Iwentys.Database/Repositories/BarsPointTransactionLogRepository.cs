using System.Linq;
using System.Threading.Tasks;
using Iwentys.Database.Context;
using Iwentys.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories
{
    public class BarsPointTransactionLogRepository : IGenericRepository<BarsPointTransactionLog, int>
    {
        private readonly IwentysDbContext _dbContext;

        public BarsPointTransactionLogRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public BarsPointTransactionLog Create(BarsPointTransactionLog entity)
        {
            EntityEntry<BarsPointTransactionLog> createdEntity = _dbContext.BarsPointTransactionLogs.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public IQueryable<BarsPointTransactionLog> Read()
        {
            return _dbContext.BarsPointTransactionLogs;
        }

        public Task<BarsPointTransactionLog> ReadById(int key)
        {
            return _dbContext.BarsPointTransactionLogs.FirstOrDefaultAsync(v => v.Id == key);
        }

        public async Task<BarsPointTransactionLog> Update(BarsPointTransactionLog entity)
        {
            EntityEntry<BarsPointTransactionLog> createdEntity = _dbContext.BarsPointTransactionLogs.Update(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public Task<int> Delete(int key)
        {
            _dbContext.BarsPointTransactionLogs.Remove(this.Get(key));
            return _dbContext.SaveChangesAsync();
        }
    }
}