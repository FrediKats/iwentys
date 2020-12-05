using System.Linq;
using System.Threading.Tasks;
using Iwentys.Database.Context;
using Iwentys.Features.StudentFeature.Domain;
using Iwentys.Features.StudentFeature.Entities;
using Iwentys.Features.StudentFeature.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Study
{
    public class StudyGroupRepository : IStudyGroupRepository
    {
        private readonly IwentysDbContext _dbContext;

        public StudyGroupRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<StudyGroupEntity> Read()
        {
            return _dbContext.StudyGroups
                .Include(s => s.Students)
                .Include(s => s.GroupSubjects)
                .ThenInclude(gs => gs.Subject)
                .Include(s => s.StudyCourseEntity)
                .ThenInclude(s => s.StudyProgramEntity);
        }

        public Task<StudyGroupEntity> ReadByIdAsync(int key)
        {
            return Read().FirstOrDefaultAsync(s => s.Id == key);
        }

        public async Task<StudyGroupEntity> UpdateAsync(StudyGroupEntity entity)
        {
            EntityEntry<StudyGroupEntity> createdEntity = _dbContext.StudyGroups.Update(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public Task<int> DeleteAsync(int key)
        {
            return _dbContext.StudyGroups.Where(sg => sg.Id == key).DeleteFromQueryAsync();
        }

        public Task<StudyGroupEntity> ReadByNamePattern(GroupName group)
        {
            return Read().FirstOrDefaultAsync(s => s.GroupName == group.Name);
        }

        public async Task<StudyGroupEntity> Create(StudyGroupEntity entity)
        {
            EntityEntry<StudyGroupEntity> createdEntity = await _dbContext.StudyGroups.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }
    }
}