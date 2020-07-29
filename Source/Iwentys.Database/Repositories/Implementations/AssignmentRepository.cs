using System;
using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities.Assignments;

namespace Iwentys.Database.Repositories.Implementations
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly IwentysDbContext _dbContext;

        public AssignmentRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Assignment Create(Assignment entity)
        {
            var insertedEntity = _dbContext.Assignments.Add(entity).Entity;
            _dbContext.SaveChanges();

            return insertedEntity;
        }

        public IQueryable<Assignment> Read()
        {
            return _dbContext.Assignments;
        }

        public Assignment ReadById(Int32 key)
        {
            return _dbContext.Assignments.Find(key);
        }

        public Assignment Update(Assignment entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Int32 key)
        {
            _dbContext.Assignments.Remove(this.Get(key));
            _dbContext.SaveChanges();
        }
    }
}