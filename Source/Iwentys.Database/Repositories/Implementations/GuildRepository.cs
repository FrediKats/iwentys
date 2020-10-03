using System.Collections.Generic;
using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Transferable.Guilds;
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

        public GuildEntity ReadById(int key)
        {
            return _dbContext.Guilds
                .Include(g => g.Members)
                .ThenInclude(gm => gm.Member)
                .Include(g => g.PinnedProjects)
                .Include(g => g.Achievements)
                .ThenInclude(a => a.Achievement)
                .Include(g => g.TestTasks)
                .FirstOrDefault(g => g.Id == key);
        }

        public GuildEntity Update(GuildEntity entity)
        {
            EntityEntry<GuildEntity> createdEntity = _dbContext.Guilds.Update(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public void Delete(int key)
        {
            GuildEntity user = this.Get(key);
            _dbContext.Guilds.Remove(user);
            _dbContext.SaveChanges();
        }

        public GuildEntity Create(StudentEntity creator, GuildCreateArgumentDto arguments)
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

        public GuildPinnedProjectEntity PinProject(int guildId, string owner, string projectName)
        {
            EntityEntry<GuildPinnedProjectEntity> entry = _dbContext.GuildPinnedProjects.Add(new GuildPinnedProjectEntity
            {
                GuildId = guildId,
                RepositoryName = projectName,
                RepositoryOwner = owner
            });
            _dbContext.SaveChanges();
            return entry.Entity;
        }

        public void UnpinProject(int pinnedProjectId)
        {
            _dbContext.GuildPinnedProjects.Remove(_dbContext.GuildPinnedProjects.Find(pinnedProjectId));
            _dbContext.SaveChanges();
        }
    }
}