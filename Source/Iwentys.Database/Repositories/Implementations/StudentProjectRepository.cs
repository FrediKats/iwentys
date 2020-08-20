using System.Collections.Generic;
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

        public void CreateMany(IEnumerable<StudentProject> studentsProjects)
        {
            _dbContext.StudentProjects.AddRange(studentsProjects);
            _dbContext.SaveChanges();
        }

        public bool Contains(StudentProject project)
        {
            return _dbContext.StudentProjects.Contains(project);
        }

        public IEnumerable<StudentProject> FindProjectsByUserName(string username)
        {
            return Read().Where(p => p.UserName == username);
        }

        public StudentProject FindCertainProject(string username, string projectName)
        {
            return Read().SingleOrDefault(p => p.UserName == username && p.Name == projectName);
        }
    }
}