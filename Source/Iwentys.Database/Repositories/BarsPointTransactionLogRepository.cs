using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Models.Entities;
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

        public BarsPointTransactionLog ReadById(int key)
        {
            return _dbContext.BarsPointTransactionLogs.Find(key);
        }

        public BarsPointTransactionLog Update(BarsPointTransactionLog entity)
        {
            EntityEntry<BarsPointTransactionLog> createdEntity = _dbContext.BarsPointTransactionLogs.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(int key)
        {
            _dbContext.BarsPointTransactionLogs.Remove(this.Get(key));
            _dbContext.SaveChanges();
        }
    }
}