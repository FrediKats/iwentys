using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Database.Context;
using Iwentys.Models;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Github;
using Microsoft.EntityFrameworkCore;
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

        public Task<GithubProjectEntity> ReadById(long key)
        {
            return _dbContext.StudentProjects.FirstOrDefaultAsync(v => v.Id == key);
        }

        public async Task<GithubProjectEntity> Update(GithubProjectEntity entity)
        {
            EntityEntry<GithubProjectEntity> createdEntity = _dbContext.StudentProjects.Update(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public Task<int> Delete(long key)
        {
            _dbContext.StudentProjects.Remove(this.Get(key));
            return _dbContext.SaveChangesAsync();
        }

        public async Task<GithubProjectEntity> GetOrCreate(GithubRepository project, StudentEntity creator)
        {
            GithubProjectEntity githubProjectEntity = await ReadById(project.Id);
            return githubProjectEntity ?? Create(new GithubProjectEntity(creator, project));
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