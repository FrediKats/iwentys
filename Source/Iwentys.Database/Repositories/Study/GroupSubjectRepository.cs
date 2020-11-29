using System.Linq;
using System.Threading.Tasks;
using Iwentys.Database.Context;
using Iwentys.Features.StudentFeature.Repositories;
using Iwentys.Models;
using Iwentys.Models.Entities.Study;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Study
{
    public class GroupSubjectRepository : IGroupSubjectRepository
    {
        private readonly IwentysDbContext _dbContext;

        public GroupSubjectRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<GroupSubjectEntity> Read()
        {
            return _dbContext.GroupSubjects
                .Include(s => s.StudyGroup)
                .Include(s => s.Subject);
        }

        public Task<GroupSubjectEntity> ReadByIdAsync(int key)
        {
            return _dbContext.GroupSubjects.FirstOrDefaultAsync(v => v.Id == key);
        }

        public async Task<GroupSubjectEntity> UpdateAsync(GroupSubjectEntity entity)
        {
            EntityEntry<GroupSubjectEntity> createdEntity = _dbContext.GroupSubjects.Update(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public Task<int> DeleteAsync(int key)
        {
            return _dbContext.GroupSubjects.Where(gs => gs.Id == key).DeleteFromQueryAsync();
        }

        public IQueryable<SubjectEntity> GetSubjectsForDto(StudySearchParameters searchParameters)
        {
            IQueryable<GroupSubjectEntity> query = Read();

            if (searchParameters.GroupId != null)
                query = query.Where(s => s.StudyGroupId == searchParameters.GroupId.Value);

            if (searchParameters.StudySemester != null)
                query = query.Where(s => s.StudySemester == searchParameters.StudySemester.Value);

            if (searchParameters.SubjectId != null)
                query = query.Where(s => s.SubjectId == searchParameters.SubjectId.Value);

            if (searchParameters.CourseId != null)
                query = query.Where(gs => gs.StudyGroup.StudyCourseId == searchParameters.CourseId);
            
            return query
                .Select(s => s.Subject)
                .Distinct();
        }

        public IQueryable<StudyGroupEntity> GetStudyGroupsForDto(int? courseId)
        {
            IQueryable<StudyGroupEntity> query = _dbContext.StudyGroups;

            if (courseId != null)
                query = query.Where(g => g.StudyCourseId == courseId);

            return query;
        }

        public GroupSubjectEntity Create(GroupSubjectEntity entity)
        {
            EntityEntry<GroupSubjectEntity> createdEntity = _dbContext.GroupSubjects.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }
    }
}