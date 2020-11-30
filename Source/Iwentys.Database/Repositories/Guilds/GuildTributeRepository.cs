using System.Linq;
using System.Threading.Tasks;
using Iwentys.Database.Context;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Repositories;
using Iwentys.Models.Entities.Github;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Guilds
{
    public class GuildTributeRepository : IGuildTributeRepository
    {
        private readonly IwentysDbContext _dbContext;

        public GuildTributeRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<TributeEntity> Read()
        {
            return _dbContext.Tributes;
        }

        public Task<TributeEntity> ReadByIdAsync(long key)
        {
            return _dbContext.Tributes.FirstOrDefaultAsync(v => v.ProjectId == key);
        }

        public async Task<TributeEntity> UpdateAsync(TributeEntity entity)
        {
            EntityEntry<TributeEntity> createdEntity = _dbContext.Tributes.Update(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public Task<int> DeleteAsync(long key)
        {
            return _dbContext.Tributes.Where(t => t.ProjectId == key).DeleteFromQueryAsync();
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