using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities.Github;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Implementations
{
    public class GithubUserRepository : IGithubUserRepository
    {
        private readonly IwentysDbContext _dbContext;

        public GithubUserRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GithubUser Create(GithubUser entity)
        {
            EntityEntry<GithubUser> createdEntry = _dbContext.GithubUsers.Add(entity);
            _dbContext.SaveChanges();
            return createdEntry.Entity;
        }

        public IQueryable<GithubUser> Read()
        {
            return _dbContext.GithubUsers;
        }

        public GithubUser ReadById(int key)
        {
            return _dbContext.GithubUsers.Find(key);
        }

        public GithubUser Update(GithubUser entity)
        {
            EntityEntry<GithubUser> createdEntry = _dbContext.GithubUsers.Update(entity);
            _dbContext.SaveChanges();
            return createdEntry.Entity;
        }

        public void Delete(int key)
        {
            _dbContext.GithubUsers.Remove(this.Get(key));
            _dbContext.SaveChanges();
        }
    }
}
