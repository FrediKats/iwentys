using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Types.Github;
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

        public StudentProject ReadById(long key)
        {
            return _dbContext.StudentProjects.Find(key);
        }

        public StudentProject Update(StudentProject entity)
        {
            EntityEntry<StudentProject> createdEntity = _dbContext.StudentProjects.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(long key)
        {
            _dbContext.StudentProjects.Remove(this.Get(key));
            _dbContext.SaveChanges();
        }

        public StudentProject GetOrCreate(GithubRepository project, Student creator)
        {
            return ReadById(project.Id) ?? Create(new StudentProject
            {
                Id = project.Id,
                Author = creator.GithubUsername,
                Description = project.Description,
                FullUrl = project.Url,
                Name = project.Name,
                Student = creator,
                StudentId = creator.Id
            });
        }
    }
}