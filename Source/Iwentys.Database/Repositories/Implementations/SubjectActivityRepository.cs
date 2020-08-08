using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities.Study;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Implementations
{
    public class SubjectActivityRepository : ISubjectActivityRepository
    {
        private readonly IwentysDbContext _dbContext;

        public SubjectActivityRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public SubjectActivity Create(SubjectActivity entity)
        {
            EntityEntry <SubjectActivity> createdEntity = _dbContext.SubjectActivities.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public IQueryable<SubjectActivity> Read()
        {
            return _dbContext.SubjectActivities;
        }

        public SubjectActivity ReadById(int key)
        {
            return _dbContext.SubjectActivities.Find(key);
        }

        public SubjectActivity Update(SubjectActivity entity)
        {
            EntityEntry<SubjectActivity> createdEntity = _dbContext.SubjectActivities.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(int key)
        {
            SubjectActivity activity = this.Get(key);
            _dbContext.SubjectActivities.Remove(activity);
            _dbContext.SaveChanges();
        }

        public SubjectActivity GetSubjectActivityForStudentAndSubjectForGroup(int studentId, int subjectForGroupId)
        {
            return Read().FirstOrDefault(s => s.StudentId == studentId && s.SubjectForGroupId == subjectForGroupId);
        }
    }
}
