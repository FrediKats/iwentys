using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
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

        public Student Create(Student entity)
        {
            EntityEntry<Student> createdEntity = _dbContext.Students.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public Student[] Read()
        {
            return _dbContext.Students.ToArray();
        }

        public Student ReadById(int key)
        {
            return _dbContext.Students.Find(key);
        }

        public Student Update(Student entity)
        {
            EntityEntry<Student> createdEntity = _dbContext.Students.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(int key)
        {
            Student user = this.Get(key);
            _dbContext.Students.Remove(user);
            _dbContext.SaveChanges();
        }
    }
}