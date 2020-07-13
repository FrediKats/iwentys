using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Implementations
{
    public class StudentProjectRepository : IStudentProjectRepository
    {
        private readonly IwentysDbContext _dbContext;

        public StudentProjectRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public StudentProject Create(StudentProject entity)
        {
            EntityEntry<StudentProject> createdEntity = _dbContext.StudentProjects.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public IQueryable<StudentProject> Read()
        {
            return _dbContext.StudentProjects;
        }

        public StudentProject ReadById(int key)
        {
            return _dbContext.StudentProjects.Find(key);
        }

        public StudentProject Update(StudentProject entity)
        {
            EntityEntry<StudentProject> createdEntity = _dbContext.StudentProjects.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(int key)
        {
            _dbContext.StudentProjects.Remove(this.Get(key));
            _dbContext.SaveChanges();
        }
    }
}