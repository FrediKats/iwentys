using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Implementations
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private IwentysDbContext _dbContext;

        public UserProfileRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public UserProfile Create(UserProfile entity)
        {
            EntityEntry<UserProfile> createdEntity = _dbContext.UserProfile.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public UserProfile[] Read()
        {
            return _dbContext.UserProfile.ToArray();
        }

        public UserProfile ReadById(int key)
        {
            return _dbContext.UserProfile.Find(key);
        }

        public UserProfile Update(UserProfile entity)
        {
            EntityEntry<UserProfile> createdEntity = _dbContext.UserProfile.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(int key)
        {
            UserProfile user = this.Get(key);
            _dbContext.UserProfile.Remove(user);
            _dbContext.SaveChanges();
        }
    }
}