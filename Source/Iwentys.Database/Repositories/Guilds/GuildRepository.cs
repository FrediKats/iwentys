using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Database.Context;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Guilds.Repositories;
using Iwentys.Features.Guilds.ViewModels.Guilds;
using Iwentys.Features.StudentFeature.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Guilds
{
    public class GuildRepository : IGuildRepository
    {
        private readonly IwentysDbContext _dbContext;

        public GuildRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<GuildEntity> Read()
        {
            return _dbContext.Guilds
                .Include(g => g.Members)
                .ThenInclude(gm => gm.Member)
                .Include(g => g.PinnedProjects)
                .Include(g => g.Achievements)
                .ThenInclude(a => a.Achievement)
                .Include(g => g.TestTasks)
                .Where(g => g.GuildType == GuildType.Created);
        }

        public Task<GuildEntity> ReadByIdAsync(int key)
        {
            return _dbContext.Guilds
                .Include(g => g.Members)
                .ThenInclude(gm => gm.Member)
                .Include(g => g.PinnedProjects)
                .Include(g => g.Achievements)
                .ThenInclude(a => a.Achievement)
                .Include(g => g.TestTasks)
                .FirstOrDefaultAsync(g => g.Id == key);
        }

        public async Task<GuildEntity> UpdateAsync(GuildEntity entity)
        {
            EntityEntry<GuildEntity> createdEntity = _dbContext.Guilds.Update(entity);
            await _dbContext.SaveChangesAsync();
            return createdEntity.Entity;
        }

        public Task<int> DeleteAsync(int key)
        {
            return _dbContext.Guilds.Where(g => g.Id == key).DeleteFromQueryAsync();
        }

        public GuildEntity Create(StudentEntity creator, GuildCreateRequest arguments)
        {
            var newGuild = new GuildEntity
            {
                Bio = arguments.Bio,
                HiringPolicy = arguments.HiringPolicy,
                LogoUrl = arguments.LogoUrl,
                Title = arguments.Title,
                GuildType = GuildType.Pending
            };

            newGuild.Members = new List<GuildMemberEntity>
            {
                new GuildMemberEntity(newGuild, creator, GuildMemberType.Creator)
            };

            EntityEntry<GuildEntity> createdEntity = _dbContext.Guilds.Add(newGuild);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public GuildEntity[] ReadPending()
        {
            return _dbContext.Guilds
                .Include(g => g.Members)
                .Include(g => g.PinnedProjects)
                .Where(g => g.GuildType == GuildType.Pending)
                .ToArray();
        }

        public GuildEntity ReadForStudent(int studentId)
        {
            return _dbContext.GuildMembers
                .Where(gm => gm.MemberId == studentId)
                .WhereIsMember()
                .Include(gm => gm.Guild.Members)
                .Include(gm => gm.Guild.PinnedProjects)
                .Select(gm => gm.Guild)
                .SingleOrDefault();
        }

        public async Task<GuildPinnedProjectEntity> PinProjectAsync(int guildId, string owner, string projectName)
        {
            EntityEntry<GuildPinnedProjectEntity> entry = await _dbContext.GuildPinnedProjects.AddAsync(new GuildPinnedProjectEntity
            {
                GuildId = guildId,
                RepositoryName = projectName,
                RepositoryOwner = owner
            });
            await _dbContext.SaveChangesAsync();
            return entry.Entity;
        }

        public void UnpinProject(int pinnedProjectId)
        {
            _dbContext.GuildPinnedProjects.Remove(_dbContext.GuildPinnedProjects.Find(pinnedProjectId));
            _dbContext.SaveChanges();
        }
    }
}