using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Entities;
using Iwentys.Database.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Implementations
{
    public class GuildProfileRepository : IGuildProfileRepository
    {
        private readonly IwentysDbContext _dbContext;

        public GuildProfileRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GuildProfile Create(GuildProfile entity)
        {
            EntityEntry<GuildProfile> createdEntity = _dbContext.GuildProfiles.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public GuildProfile[] Read()
        {
            return _dbContext.GuildProfiles.ToArray();
        }

        public GuildProfile ReadById(int key)
        {
            return _dbContext.GuildProfiles.Find(key);
        }

        public GuildProfile Update(GuildProfile entity)
        {
            EntityEntry<GuildProfile> createdEntity = _dbContext.GuildProfiles.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(int key)
        {
            GuildProfile user = this.Get(key);
            _dbContext.GuildProfiles.Remove(user);
            _dbContext.SaveChanges();
        }
    }
}