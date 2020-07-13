using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Types.Guilds;
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
            EntityEntry<Guild> createdEntity = _dbContext.Guilds.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public IQueryable<Guild> Read()
        {
            return _dbContext.Guilds
                .Include(g => g.Members)
                .Where(g => g.GuildType == GuildType.Created);
        }

        public Guild ReadById(int key)
        {
            return _dbContext.Guilds
                .Include(g => g.Members)
                .FirstOrDefault(g => g.Id == key);
        }

        public Guild Update(Guild entity)
        {
            EntityEntry<Guild> createdEntity = _dbContext.Guilds.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(int key)
        {
            Guild user = this.Get(key);
            _dbContext.Guilds.Remove(user);
            _dbContext.SaveChanges();
        }

        public Guild[] ReadPending()
        {
            return _dbContext.Guilds
                .Include(g => g.Members)
                .Where(g => g.GuildType == GuildType.Pending)
                .ToArray();
        }

        public Guild ReadForStudent(int studentId)
        {
            return _dbContext.GuildMembers
                .Where(gm => gm.MemberId == studentId)
                .Include(gm => gm.Guild)
                .SingleOrDefault()
                ?.Guild;
        }

        public Guild ReadForTotem(int totemId)
        {
            return _dbContext.Guilds.SingleOrDefault(g => g.TotemId == totemId);
        }
    }
}