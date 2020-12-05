using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Database.Context;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models;
using Iwentys.Features.Study.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Study
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
                .Include(s => s.GroupSubject)
                .ThenInclude(s => s.Subject);
        }

        public async Task<SubjectActivityEntity> UpdateAsync(SubjectActivityEntity entity)
        {
            EntityEntry<SubjectActivityEntity> createdEntity = _dbContext.SubjectActivities.Update(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public IReadOnlyCollection<SubjectActivityEntity> GetStudentActivities(StudySearchParametersDto searchParametersDto)
        {
            var query =
                from sa in Read()
                join sg in _dbContext.StudyGroups on sa.Student.GroupId equals sg.Id
                join gs in _dbContext.GroupSubjects on sa.GroupSubjectEntityId equals gs.Id
                select new { SubjectActivities = sa, StudyGroups = sg, GroupSubjects = gs };

            query = query
                .WhereIf(searchParametersDto.GroupId, q => q.StudyGroups.Id == searchParametersDto.GroupId)
                .WhereIf(searchParametersDto.SubjectId, q => q.GroupSubjects.SubjectId == searchParametersDto.SubjectId)
                .WhereIf(searchParametersDto.CourseId, q => q.StudyGroups.StudyCourseId == searchParametersDto.CourseId)
                .WhereIf(searchParametersDto.StudySemester, q => q.GroupSubjects.StudySemester == searchParametersDto.StudySemester);

            return query
                .Select(_ => _.SubjectActivities)
                .ToList();
        }
    }
}