using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Database.Context;
using Iwentys.Models.Entities;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Tools;
using Iwentys.Models.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories
{
    public class CompanyRepository : IGenericRepository<CompanyEntity, int>
    {
        private readonly IwentysDbContext _dbContext;

        public CompanyRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CompanyEntity> CreateAsync(CompanyEntity entity)
        {
            EntityEntry<CompanyEntity> createdEntity = await _dbContext.Companies.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public IQueryable<CompanyEntity> Read()
        {
            return _dbContext.Companies;
        }

        public Task<CompanyEntity> ReadByIdAsync(int key)
        {
            return _dbContext
                .Companies
                .FirstOrDefaultAsync(v => v.Id == key);
        }

        public async Task<CompanyEntity> UpdateAsync(CompanyEntity entity)
        {
            EntityEntry<CompanyEntity> createdEntity = _dbContext.Companies.Update(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public Task<int> DeleteAsync(int key)
        {
            return _dbContext.Companies.Where(c => c.Id == key).DeleteFromQueryAsync();
        }

        public Task<List<StudentEntity>> ReadWorkersAsync(CompanyEntity companyEntity)
        {
            return _dbContext
                .CompanyWorkers
                .Where(cw => cw.CompanyId == companyEntity.Id)
                .Where(cw => cw.Type == CompanyWorkerType.Accepted)
                .Select(cw => cw.Worker)
                .ToListAsync();
        }

        public Task<List<CompanyWorkerEntity>> ReadWorkerRequestAsync()
        {
            return _dbContext
                .CompanyWorkers
                .Where(cw => cw.Type == CompanyWorkerType.Requested)
                .Include(cw => cw.Worker)
                .Include(cw => cw.CompanyEntity)
                .ToListAsync();
        }

        public async Task AddCompanyWorkerRequestAsync(CompanyEntity companyEntity, StudentEntity worker)
        {
            List<CompanyWorkerEntity> workerRequests = await ReadWorkerRequestAsync();
            if (workerRequests.Any(r => r.WorkerId == worker.Id))
                throw new InnerLogicException("Student already request adding to company");

            await _dbContext.CompanyWorkers.AddAsync(CompanyWorkerEntity.NewRequest(companyEntity, worker));
            await _dbContext.SaveChangesAsync();
        }

        public async Task ApproveRequestAsync(StudentEntity user)
        {
            CompanyWorkerEntity workerEntity = await _dbContext.CompanyWorkers.SingleOrDefaultAsync(cw => cw.WorkerId == user.Id);
            if (workerEntity == null)
                throw EntityNotFoundException.Create(nameof(CompanyWorkerEntity), user.Id);

            workerEntity.Type = CompanyWorkerType.Accepted;
            _dbContext.CompanyWorkers.Update(workerEntity);
            await _dbContext.SaveChangesAsync();
        }
    }
}