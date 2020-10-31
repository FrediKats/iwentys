using System.Linq;
using System.Threading.Tasks;
using Iwentys.Database.Context;
using Iwentys.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories
{
    public class StudentRepository : IGenericRepository<StudentEntity, int>
    {
        private readonly IwentysDbContext _dbContext;

        public StudentRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public StudentEntity Create(StudentEntity entity)
        {
            EntityEntry<StudentEntity> createdEntity = _dbContext.Students.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public IQueryable<StudentEntity> Read()
        {
            return _dbContext.Students
                .Include(s => s.Group)
                .Include(s => s.Achievements)
                .ThenInclude(a => a.Achievement)
                .Include(s => s.SubjectActivities)
                .ThenInclude(a => a.GroupSubject)
                .ThenInclude(sg => sg.Subject)
                .Include(s => s.GithubUserEntity)
                .Include(s => s.GuildMember)
                .ThenInclude(gm => gm.Guild);
        }

        public Task<StudentEntity> ReadById(int key)
        {
            return Read().FirstOrDefaultAsync(s => s.Id == key);
        }

        public async Task<StudentEntity> Update(StudentEntity entity)
        {
            EntityEntry<StudentEntity> createdEntity = _dbContext.Students.Update(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public Task<int> Delete(int key)
        {
            StudentEntity user = this.Get(key);
            _dbContext.Students.Remove(user);
            return _dbContext.SaveChangesAsync();
        }
    }
}