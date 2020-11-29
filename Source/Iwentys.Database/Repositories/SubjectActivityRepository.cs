using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Database.Context;
using Iwentys.Features.StudentFeature.Repositories;
using Iwentys.Models;
using Iwentys.Models.Entities.Study;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories
{
    public class SubjectActivityRepository : ISubjectActivityRepository
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

        public async Task<SubjectActivityEntity> UpdateAsync(SubjectActivityEntity entity)
        {
            EntityEntry<SubjectActivityEntity> createdEntity = _dbContext.SubjectActivities.Update(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public IReadOnlyCollection<SubjectActivityEntity> GetStudentActivities(StudySearchParameters searchParameters)
        {
            var query =
                from sa in Read()
                join sg in _dbContext.StudyGroups on sa.Student.GroupId equals sg.Id
                join gs in _dbContext.GroupSubjects on sa.GroupSubjectEntityId equals gs.Id
                select new { SubjectActivities = sa, StudyGroups = sg, GroupSubjects = gs };

            query = query
                .WhereIf(searchParameters.GroupId, q => q.StudyGroups.Id == searchParameters.GroupId)
                .WhereIf(searchParameters.SubjectId, q => q.GroupSubjects.SubjectId == searchParameters.SubjectId)
                .WhereIf(searchParameters.CourseId, q => q.StudyGroups.StudyCourseId == searchParameters.CourseId)
                .WhereIf(searchParameters.StudySemester, q => q.GroupSubjects.StudySemester == searchParameters.StudySemester);

            return query
                .Select(_ => _.SubjectActivities)
                .ToList();
        }
    }
}