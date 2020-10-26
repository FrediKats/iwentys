using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Models.Entities.Github;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories
{
    public class GithubUserDataRepository : IGenericRepository<GithubUserEntity, int>
    {
        private readonly IwentysDbContext _dbContext;

        public GithubUserDataRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GithubUserEntity Create(GithubUserEntity entity)
        {
            EntityEntry<GithubUserEntity> createdEntry = _dbContext.GithubUsersData.Add(entity);
            _dbContext.SaveChanges();
            return createdEntry.Entity;
        }

        public IQueryable<GithubUserEntity> Read()
        {
            return _dbContext.GithubUsersData;
        }

        public GithubUserEntity ReadById(int key)
        {
            return _dbContext.GithubUsersData.Find(key);
        }

        public GithubUserEntity Update(GithubUserEntity entity)
        {
            EntityEntry<GithubUserEntity> createdEntry = _dbContext.GithubUsersData.Update(entity);
            _dbContext.SaveChanges();
            return createdEntry.Entity;
        }

        public void Delete(int key)
        {
            _dbContext.GithubUsersData.Remove(this.Get(key));
            _dbContext.SaveChanges();
        }

        public GithubUserEntity FindByUsername(string username)
        {
            return Read().SingleOrDefault(g => g.Username == username);
        }
    }
}