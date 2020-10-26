using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Models;
using Iwentys.Models.Entities.Study;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories
{
    public class StudyGroupRepository : IGenericRepository<StudyGroupEntity, int>
    {
        private readonly IwentysDbContext _dbContext;

        public StudyGroupRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<StudyGroupEntity> Read()
        {
            return _dbContext.StudyGroups
                .Include(s => s.StudyCourseEntity)
                .ThenInclude(s => s.StudyProgramEntity);
        }

        public StudyGroupEntity ReadById(int key)
        {
            return Read().FirstOrDefault(s => s.Id == key);
        }

        public StudyGroupEntity Update(StudyGroupEntity entity)
        {
            EntityEntry<StudyGroupEntity> createdEntity = _dbContext.StudyGroups.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(int key)
        {
            StudyGroupEntity studyGroup = this.Get(key);
            _dbContext.StudyGroups.Remove(studyGroup);
            _dbContext.SaveChanges();
        }

        public StudyGroupEntity ReadByNamePattern(GroupName group)
        {
            return Read().FirstOrDefault(s => s.GroupName == group.Name);
        }

        public StudyGroupEntity Create(StudyGroupEntity entity)
        {
            EntityEntry<StudyGroupEntity> createdEntity = _dbContext.StudyGroups.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }
    }
}