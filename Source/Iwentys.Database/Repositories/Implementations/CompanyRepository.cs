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

        public Company Create(Company entity)
        {
            EntityEntry<Company> createdEntity = _dbContext.Companies.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public IQueryable<Company> Read()
        {
            return _dbContext.Companies;
        }

        public Company ReadById(int key)
        {
            return _dbContext
                .Companies
                .Find(key);
        }

        public Company Update(Company entity)
        {
            EntityEntry<Company> createdEntity = _dbContext.Companies.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(int key)
        {
            Company entity = this.Get(key);
            _dbContext.Companies.Remove(entity);
            _dbContext.SaveChanges();
        }

        public StudentEntity[] ReadWorkers(Company company)
        {
            return _dbContext
                .CompanyWorkers
                .Where(cw => cw.CompanyId == company.Id)
                .Where(cw => cw.Type == CompanyWorkerType.Accepted)
                .Select(cw => cw.Worker)
                .ToArray();
        }

        public CompanyWorker[] ReadWorkerRequest()
        {
            return _dbContext
                .CompanyWorkers
                .Where(cw => cw.Type == CompanyWorkerType.Requested)
                .Include(cw => cw.Worker)
                .Include(cw => cw.Company)
                .ToArray();
        }

        public void AddCompanyWorkerRequest(Company company, StudentEntity worker)
        {
            if (ReadWorkerRequest().Any(r => r.WorkerId == worker.Id))
                throw new InnerLogicException("Student already request adding to company");

            _dbContext.CompanyWorkers.Add(CompanyWorker.NewRequest(company, worker));
            _dbContext.SaveChanges();
        }

        public void ApproveRequest(StudentEntity user)
        {
            CompanyWorker worker = _dbContext.CompanyWorkers.SingleOrDefault(cw => cw.WorkerId == user.Id) ?? throw EntityNotFoundException.Create(nameof(CompanyWorker), user.Id);
            worker.Type = CompanyWorkerType.Accepted;
            _dbContext.CompanyWorkers.Update(worker);
            _dbContext.SaveChanges();
        }
    }
}