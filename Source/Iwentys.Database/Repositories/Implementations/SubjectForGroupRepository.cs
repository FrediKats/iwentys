using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities.Study;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Implementations
{
    class SubjectForGroupRepository : ISubjectForGroupRepository
    {
        private readonly IwentysDbContext _dbContext;

        public SubjectForGroupRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public SubjectForGroup Create(SubjectForGroup entity)
        {
            EntityEntry<SubjectForGroup> createdEntity = _dbContext.SubjectForGroups.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public IQueryable<SubjectForGroup> Read()
        {
            return _dbContext.SubjectForGroups;
        }

        public SubjectForGroup ReadById(int key)
        {
            return _dbContext.SubjectForGroups.Find(key);
        }

        public SubjectForGroup Update(SubjectForGroup entity)
        {
            EntityEntry<SubjectForGroup> createdEntity = _dbContext.SubjectForGroups.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(int key)
        {
            SubjectForGroup subjectForGroup = this.Get(key);
            _dbContext.SubjectForGroups.Remove(subjectForGroup);
            _dbContext.SaveChanges();
        }
    }
}
