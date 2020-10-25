using System.Collections.Generic;
using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Models;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Github;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories
{
    public class StudentProjectRepository : IGenericRepository<GithubProjectEntity, long>
    {
        private readonly IwentysDbContext _dbContext;

        public StudentProjectRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GithubProjectEntity Create(GithubProjectEntity entity)
        {
            EntityEntry<GithubProjectEntity> createdEntity = _dbContext.StudentProjects.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public IQueryable<GithubProjectEntity> Read()
        {
            return _dbContext.StudentProjects;
        }

        public GithubProjectEntity ReadById(long key)
        {
            return _dbContext.StudentProjects.Find(key);
        }

        public GithubProjectEntity Update(GithubProjectEntity entity)
        {
            EntityEntry<GithubProjectEntity> createdEntity = _dbContext.StudentProjects.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(long key)
        {
            _dbContext.StudentProjects.Remove(this.Get(key));
            _dbContext.SaveChanges();
        }

        public GithubProjectEntity GetOrCreate(GithubRepository project, StudentEntity creator)
        {
            return ReadById(project.Id) ?? Create(new GithubProjectEntity(creator, project));
        }

        public void CreateMany(IEnumerable<GithubProjectEntity> studentsProjects)
        {
            _dbContext.StudentProjects.AddRange(studentsProjects);
            _dbContext.SaveChanges();
        }

        public bool Contains(GithubProjectEntity projectEntity)
        {
            return _dbContext.StudentProjects.Contains(projectEntity);
        }

        public IEnumerable<GithubProjectEntity> FindProjectsByUserName(string username)
        {
            return Read().Where(p => p.UserName == username);
        }

        public GithubProjectEntity FindCertainProject(string username, string projectName)
        {
            return Read().SingleOrDefault(p => p.UserName == username && p.Name == projectName);
        }
    }
}