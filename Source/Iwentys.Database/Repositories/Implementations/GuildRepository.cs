using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Implementations
{
    public class GuildRepository : IGuildRepository
    {
        private readonly IwentysDbContext _dbContext;

        public GuildRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Guild Create(Guild entity)
        {
            EntityEntry<Guild> createdEntity = _dbContext.GuildProfiles.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public Guild[] Read()
        {
            return _dbContext.GuildProfiles
                .Include(g => g.Members)
                .Where(g => g.GuildType == GuildType.Created)
                .ToArray();
        }

        public Guild ReadById(int key)
        {
            return _dbContext.GuildProfiles
                .Include(g => g.Members)
                .FirstOrDefault(g => g.Id == key);
        }

        public Guild Update(Guild entity)
        {
            EntityEntry<Guild> createdEntity = _dbContext.GuildProfiles.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(int key)
        {
            Guild user = this.Get(key);
            _dbContext.GuildProfiles.Remove(user);
            _dbContext.SaveChanges();
        }

        public Guild[] ReadPending()
        {
            return _dbContext.GuildProfiles
                .Include(g => g.Members)
                .Where(g => g.GuildType == GuildType.Pending)
                .ToArray();
        }

        public Guild ReadForUser(int userId)
        {
            return _dbContext.GuildMembers
                .Where(gm => gm.MemberId == userId)
                .Include(gm => gm.Guild)
                .SingleOrDefault()
                ?.Guild;
        }
    }
}