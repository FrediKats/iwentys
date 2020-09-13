using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Implementations
{
    public class StudentRepository : IStudentRepository
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
                .ThenInclude(a => a.GroupSubjectEntity)
                .ThenInclude(sg => sg.Subject)
                .Include(s => s.GuildMember)
                .ThenInclude(gm => gm.Guild);
        }

        public StudentEntity ReadById(int key)
        {
            return Read().FirstOrDefault(s => s.Id == key);
        }

        public StudentEntity Update(StudentEntity entity)
        {
            EntityEntry<StudentEntity> createdEntity = _dbContext.Students.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(int key)
        {
            StudentEntity user = this.Get(key);
            _dbContext.Students.Remove(user);
            _dbContext.SaveChanges();
        }
    }
}