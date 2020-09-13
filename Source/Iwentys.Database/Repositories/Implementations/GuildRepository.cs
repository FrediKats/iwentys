using System;
using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Exceptions;
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

        public GuildEntity Create(GuildEntity entity)
        {
            EntityEntry<GuildEntity> createdEntity = _dbContext.Guilds.Add(entity);
            _dbContext.SaveChanges();
            return createdEntity.Entity;
        }

        public IQueryable<GuildEntity> Read()
        {
            return _dbContext.Guilds
                .Include(g => g.Members)
                .ThenInclude(gm => gm.Member)
                .Include(g => g.PinnedProjects)
                .Include(g => g.Achievements)
                .ThenInclude(a => a.Achievement)
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

        public Boolean IsStudentHaveRequest(Int32 studentId)
        {
            return !_dbContext.GuildMembers
                .Where(m=> m.Member.Id == studentId)
                .Any(m => m.MemberType == GuildMemberType.Requested);
        }

        public GuildMember AddMember(GuildMember member)
        {
            EntityEntry<GuildMember> addedEntity =  _dbContext.GuildMembers.Add(member);

            _dbContext.SaveChanges();

            return addedEntity.Entity;
        }

        public GuildMember UpdateMember(GuildMember member)
        {
            EntityEntry<GuildMember> updatedEntity =  _dbContext.GuildMembers.Update(member);

            _dbContext.SaveChanges();

            return updatedEntity.Entity;
        }

        public void RemoveMember(int guildId, int userId)
        {
            GuildMember guildMember = _dbContext.GuildMembers.Single(gm => gm.GuildId == guildId && gm.MemberId == userId);
            if (guildMember.MemberType == GuildMemberType.Creator)
                throw InnerLogicException.Guild.CreatorCannotLeave(userId, guildId);
            _dbContext.GuildMembers.Remove(guildMember);

            _dbContext.SaveChanges();
        }

        public GuildPinnedProject PinProject(int guildId, string owner, string projectName)
        {
            EntityEntry<GuildPinnedProject> entry = _dbContext.GuildPinnedProjects.Add(new GuildPinnedProject
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