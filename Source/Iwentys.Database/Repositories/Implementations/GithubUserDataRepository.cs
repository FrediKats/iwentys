﻿using System.Collections.Generic;
using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Implementations
{
    public class GithubUserDataRepository : IGithubUserDataRepository
    {
        private readonly IwentysDbContext _dbContext;

        public GithubUserDataRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GithubUserData Create(GithubUserData entity)
        {
            EntityEntry<GithubUserData> createdEntry = _dbContext.GithubUsersData.Add(entity);
            _dbContext.SaveChanges();
            return createdEntry.Entity;
        }

        public IQueryable<GithubUserData> Read()
        {
            return _dbContext.GithubUsersData;
        }

        public GithubUserData ReadById(int key)
        {
            return _dbContext.GithubUsersData.Find(key);
        }

        public GithubUserData Update(GithubUserData entity)
        {
            EntityEntry<GithubUserData> createdEntry = _dbContext.GithubUsersData.Update(entity);
            _dbContext.SaveChanges();
            return createdEntry.Entity;
        }

        public void Delete(int key)
        {
            _dbContext.GithubUsersData.Remove(this.Get(key));
            _dbContext.SaveChanges();
        }

        public GithubUserData GetUserDataByUsername(string username)
        {
            return Read().SingleOrDefault(g => g.GithubUser.Name == username);
        }
    }
}
