using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
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

        public Company[] Read()
        {
            return _dbContext
                .Companies
                .ToArray();
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
    }
}