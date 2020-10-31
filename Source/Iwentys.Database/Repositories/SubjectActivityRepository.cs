using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Database.Context;
using Iwentys.Database.Tools;
using Iwentys.Models;
using Iwentys.Models.Entities.Study;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories
{
    public class SubjectActivityRepository : IGenericRepository<SubjectActivityEntity, int>
    {
        private readonly IwentysDbContext _dbContext;

        public SubjectActivityRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public SubjectActivityEntity Create(SubjectActivityEntity entity)
        {
            EntityEntry<SubjectActivityEntity> createdEntity = _dbContext.SubjectActivities.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public IQueryable<SubjectActivityEntity> Read()
        {
            return _dbContext.SubjectActivities
                .Include(s => s.Student)
                .Include(s => s.GroupSubject);
        }

        public SubjectActivityEntity ReadById(int key)
        {
            return _dbContext.SubjectActivities.Find(key);
        }

        public SubjectActivityEntity Update(SubjectActivityEntity entity)
        {
            EntityEntry<SubjectActivityEntity> createdEntity = _dbContext.SubjectActivities.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public Task<int> Delete(int key)
        {
            SubjectActivityEntity activity = this.Get(key);
            _dbContext.SubjectActivities.Remove(activity);
            return _dbContext.SaveChangesAsync();
        }

        public IReadOnlyCollection<SubjectActivityEntity> GetStudentActivities(StudySearchParameters searchParameters)
        {
            var query =
                from sa in Read()
                join sg in _dbContext.StudyGroups on sa.Student.GroupId equals sg.Id
                join gs in _dbContext.GroupSubjects on sa.GroupSubjectEntityId equals gs.Id
                select new { SubjectActivities = sa, StudyGroups = sg, GroupSubjects = gs };

            query = query
                .WhereIf(searchParameters.GroupId, () => query.Where(q => q.StudyGroups.Id == searchParameters.GroupId))
                .WhereIf(searchParameters.SubjectId, () => query.Where(q => q.GroupSubjects.SubjectId == searchParameters.SubjectId))
                .WhereIf(searchParameters.CourseId, () => query.Where(q => q.StudyGroups.StudyCourseId == searchParameters.CourseId))
                .WhereIf(searchParameters.StudySemester, () => query.Where(q => q.GroupSubjects.StudySemester == searchParameters.StudySemester));

            return query
                .Select(_ => _.SubjectActivities)
                .ToList();
        }
    }
}