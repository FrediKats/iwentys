using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities.Guilds;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Implementations
{
    public class TributeRepository : ITributeRepository
    {
        private readonly IwentysDbContext _dbContext;

        public TributeRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Tribute Create(Tribute entity)
        {
            EntityEntry<Tribute> createdEntity = _dbContext.Tributes.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public Tribute[] Read()
        {
            return _dbContext.Tributes.ToArray();
        }

        public Tribute ReadById(int key)
        {
            return _dbContext.Tributes.Find(key);
        }

        public Tribute Update(Tribute entity)
        {
            EntityEntry<Tribute> createdEntity = _dbContext.Tributes.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(int key)
        {
            _dbContext.Tributes.Remove(this.Get(key));
            _dbContext.SaveChanges();
        }

        public Tribute[] ReadForGuild(int guildId)
        {
            return _dbContext.Tributes.Where(t => t.GuildId == guildId).ToArray();
        }

        public Tribute[] ReadStudentInGuildTributes(int guildId, int studentId)
        {
            return _dbContext
                .Tributes
                .Where(t => t.GuildId == guildId)
                .Where(t => t.Project.StudentId == studentId)
                .ToArray();
        }
    }
}