using System.Linq;
using System.Threading.Tasks;
using Iwentys.Database.Context;
using Iwentys.Features.Economy.Repositories;
using Iwentys.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Economy
{
    public class BarsPointTransactionLogRepository : IBarsPointTransactionLogRepository
    {
        private readonly IwentysDbContext _dbContext;

        public BarsPointTransactionLogRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BarsPointTransactionLog> CreateAsync(BarsPointTransactionLog entity)
        {
            EntityEntry<BarsPointTransactionLog> createdEntity = await _dbContext.BarsPointTransactionLogs.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public IQueryable<BarsPointTransactionLog> Read()
        {
            return _dbContext.BarsPointTransactionLogs;
        }

        public Task<BarsPointTransactionLog> ReadByIdAsync(int key)
        {
            return _dbContext.BarsPointTransactionLogs.FirstOrDefaultAsync(v => v.Id == key);
        }

        public async Task<BarsPointTransactionLog> UpdateAsync(BarsPointTransactionLog entity)
        {
            EntityEntry<BarsPointTransactionLog> createdEntity = _dbContext.BarsPointTransactionLogs.Update(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public Task<int> DeleteAsync(int key)
        {
            return _dbContext.BarsPointTransactionLogs.Where(e => e.Id == key).DeleteFromQueryAsync();
        }
    }
}