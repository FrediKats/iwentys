using System.Linq;
using System.Threading.Tasks;
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