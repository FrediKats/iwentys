using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Implementations
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IwentysDbContext _dbContext;

        public CompanyRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public CompanyEntity Create(CompanyEntity entity)
        {
            EntityEntry<CompanyEntity> createdEntity = _dbContext.Companies.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public IQueryable<CompanyEntity> Read()
        {
            return _dbContext.Companies;
        }

        public CompanyEntity ReadById(int key)
        {
            return _dbContext
                .Companies
                .Find(key);
        }

        public CompanyEntity Update(CompanyEntity entity)
        {
            EntityEntry<CompanyEntity> createdEntity = _dbContext.Companies.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(int key)
        {
            CompanyEntity entity = this.Get(key);
            _dbContext.Companies.Remove(entity);
            _dbContext.SaveChanges();
        }

        public StudentEntity[] ReadWorkers(CompanyEntity companyEntity)
        {
            return _dbContext
                .CompanyWorkers
                .Where(cw => cw.CompanyId == companyEntity.Id)
                .Where(cw => cw.Type == CompanyWorkerType.Accepted)
                .Select(cw => cw.Worker)
                .ToArray();
        }

        public CompanyWorkerEntity[] ReadWorkerRequest()
        {
            return _dbContext
                .CompanyWorkers
                .Where(cw => cw.Type == CompanyWorkerType.Requested)
                .Include(cw => cw.Worker)
                .Include(cw => cw.CompanyEntity)
                .ToArray();
        }

        public void AddCompanyWorkerRequest(CompanyEntity companyEntity, StudentEntity worker)
        {
            if (ReadWorkerRequest().Any(r => r.WorkerId == worker.Id))
                throw new InnerLogicException("Student already request adding to company");

            _dbContext.CompanyWorkers.Add(CompanyWorkerEntity.NewRequest(companyEntity, worker));
            _dbContext.SaveChanges();
        }

        public void ApproveRequest(StudentEntity user)
        {
            CompanyWorkerEntity workerEntity = _dbContext.CompanyWorkers.SingleOrDefault(cw => cw.WorkerId == user.Id) ?? throw EntityNotFoundException.Create(nameof(CompanyWorkerEntity), user.Id);
            workerEntity.Type = CompanyWorkerType.Accepted;
            _dbContext.CompanyWorkers.Update(workerEntity);
            _dbContext.SaveChanges();
        }
    }
}