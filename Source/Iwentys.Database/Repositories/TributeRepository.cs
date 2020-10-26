using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Models.Entities.Github;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Types;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories
{
    public class TributeRepository : IGenericRepository<TributeEntity, long>
    {
        private readonly IwentysDbContext _dbContext;

        public TributeRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TributeEntity> Read()
        {
            return _dbContext.Tributes;
        }

        public TributeEntity ReadById(long key)
        {
            return _dbContext.Tributes.Find(key);
        }

        public TributeEntity Update(TributeEntity entity)
        {
            EntityEntry<TributeEntity> createdEntity = _dbContext.Tributes.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(long key)
        {
            _dbContext.Tributes.Remove(this.Get(key));
            _dbContext.SaveChanges();
        }

        public TributeEntity Create(GuildEntity guild, GithubProjectEntity githubProject)
        {
            EntityEntry<TributeEntity> createdEntity = _dbContext.Tributes.Add(new TributeEntity(guild, githubProject));
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public TributeEntity[] ReadForGuild(int guildId)
        {
            return _dbContext.Tributes.Where(t => t.GuildId == guildId).ToArray();
        }

        public TributeEntity[] ReadStudentInGuildTributes(int guildId, int studentId)
        {
            return _dbContext
                .Tributes
                .Where(t => t.GuildId == guildId)
                .Where(t => t.ProjectEntity.StudentId == studentId)
                .ToArray();
        }

        public TributeEntity ReadStudentActiveTribute(int guildId, int studentId)
        {
            return _dbContext
                .Tributes
                .Where(t => t.GuildId == guildId)
                .Where(t => t.ProjectEntity.StudentId == studentId)
                .SingleOrDefault(t => t.State == TributeState.Active);
        }
    }
}