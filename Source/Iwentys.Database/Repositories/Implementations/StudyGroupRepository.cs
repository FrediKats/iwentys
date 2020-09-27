using System;
using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models;
using Iwentys.Models.Entities.Study;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Implementations
{
    public class StudyGroupRepository : IStudyGroupRepository
    {
        private readonly IwentysDbContext _dbContext;

        public StudyGroupRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public StudyGroupEntity Create(StudyGroupEntity entity)
        {
            EntityEntry<StudyGroupEntity> createdEntity = _dbContext.StudyGroups.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public IQueryable<StudyGroupEntity> Read()
        {
            return _dbContext.StudyGroups
                .Include(s => s.StudyCourseEntity)
                .ThenInclude(s => s.StudyProgramEntity);
        }

        public StudyGroupEntity ReadById(Int32 key)
        {
            return Read().FirstOrDefault(s => s.Id == key);
        }

        public StudyGroupEntity Update(StudyGroupEntity entity)
        {
            EntityEntry<StudyGroupEntity> createdEntity = _dbContext.StudyGroups.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(Int32 key)
        {
            StudyGroupEntity studyGroup = this.Get(key);
            _dbContext.StudyGroups.Remove(studyGroup);
            _dbContext.SaveChanges();
        }

        public StudyGroupEntity ReadByNamePattern(GroupName group)
        {
            return Read().FirstOrDefault(s => s.GroupName == group.Name);
        }
    }
}