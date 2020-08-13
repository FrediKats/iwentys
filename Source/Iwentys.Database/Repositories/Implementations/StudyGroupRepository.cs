using System;
using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
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

        public StudyGroup Create(StudyGroup entity)
        {
            EntityEntry<StudyGroup> createdEntity = _dbContext.StudyGroups.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public IQueryable<StudyGroup> Read()
        {
            return _dbContext.StudyGroups
                .Include(s => s.StudyProgram)
                .Include(s => s.StudyStream);
        }

        public StudyGroup ReadById(Int32 key)
        {
            return Read().FirstOrDefault(s => s.Id == key);
        }

        public StudyGroup Update(StudyGroup entity)
        {
            EntityEntry<StudyGroup> createdEntity = _dbContext.StudyGroups.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(Int32 key)
        {
            StudyGroup studyGroup = this.Get(key);
            _dbContext.StudyGroups.Remove(studyGroup);
            _dbContext.SaveChanges();
        }

        public StudyGroup ReadByNamePattern(String namePattern)
        {
            return Read().FirstOrDefault(s => s.NamePattern == namePattern);
        }
    }
}