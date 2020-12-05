using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Database.Context;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.GithubIntegration.Repositories;
using Iwentys.Features.Students.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.GithubIntegration
{
    public class StudentProjectRepository : IStudentProjectRepository
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

        public Task<GithubProjectEntity> ReadByIdAsync(long key)
        {
            return _dbContext.StudentProjects.FirstOrDefaultAsync(v => v.Id == key);
        }

        public async Task<GithubProjectEntity> UpdateAsync(GithubProjectEntity entity)
        {
            EntityEntry<GithubProjectEntity> createdEntity = _dbContext.StudentProjects.Update(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public Task<int> DeleteAsync(long key)
        {
            return _dbContext.StudentProjects.Where(sp => sp.Id == key).DeleteFromQueryAsync();
        }

        public async Task<GithubProjectEntity> GetOrCreateAsync(GithubRepository project, StudentEntity creator)
        {
            GithubProjectEntity githubProjectEntity = await ReadByIdAsync(project.Id);
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